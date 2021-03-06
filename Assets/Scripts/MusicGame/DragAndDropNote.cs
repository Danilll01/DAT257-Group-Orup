using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropNote : MonoBehaviour
{
    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;
    private Vector3 originalPos;
    private Vector3 targetPosition;
    private Transform originalParent;
    private float originalColliderRadius;
    private Rigidbody2D ridgidBody;
    private SpriteRenderer sprite;
    private bool toBeDeleted;

    // What distance the program should fade helper lines
    private float helperLineCutoffDisctance = 3.5f;

    // If the note can move
    private bool lockedInPlace = false;

    // Audio source for playing the note sound
    private AudioSource audioSource;

    // Which parent to look for snap points in
    [SerializeField] private GameObject snapPointsParent;
    private GameObject[] snapPoints;

    // Which parent to look for snap points in
    [SerializeField] private GameObject helperLinesParent;
    private GameObject[] helperLines;

    // Which instrument the note is
    [SerializeField] Instrument instrument;

    // Every note for the current instrumet
    [SerializeField] private AudioClip[] instrumentClips;

    // Used to make note easier to grab when in the pool
    [SerializeField] private float spawnColliderSize = 1;

    void Start()
    {
        // Setup the original position
        originalPos = transform.position;
        targetPosition = originalPos;
        originalParent = transform.parent;

        // Initialize array with snap points
        if (snapPointsParent != null)
        {
            snapPoints = FillArrayFromParent(snapPointsParent);
        }
        
        ridgidBody = GetComponent<Rigidbody2D>();

        // Gets objects audio source
        audioSource = GetComponent<AudioSource>();

        // Dissables collision between note objects 
        Physics2D.IgnoreLayerCollision(6, 6); // Notes needs to be on layer 6

        // Get original collider size
        originalColliderRadius = GetComponent<CircleCollider2D>().radius;

        // Make the note easier to grab and disable the sprite
        GetComponent<CircleCollider2D>().radius = spawnColliderSize;
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
    }

    // -------------------- Begin note movement --------------------
    void Update()
    {
        // If there were any touches on the screen
        if (Input.touchCount > 0 && !lockedInPlace)
        {
            Touch touch = Input.GetTouch(0);

            // Get the position of the touch
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);


            switch (touch.phase)
            {
                // At the moment the screen was touched, look if the touch is on the object.
                // If the touch was on the object, set moveAllowed to true
                case TouchPhase.Began:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        BeginMove(true);
                    }
                    break;

                // While the touch is moving and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (moveAllowed)
                    {
                        OnMove(touchPos);
                    }
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    // Only move object if it should be moved
                    if (moveAllowed)
                    {
                        EndMove(touchPos);
                    }
                    moveAllowed = false;
                    break;

            }
        }

        // For mouse controls
        else if (!lockedInPlace)
        {
            // Get the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If the mouse is pressed down and the mouse is over the object, set mouseMoveAllowed to true
            if (Input.GetMouseButtonDown(0) && GetComponent<Collider2D>() == Physics2D.OverlapPoint(mousePosition))
            {
                BeginMove(false);
            }

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if (mouseMoveAllowed)
            {
                OnMove(mousePosition);

                if (Input.GetMouseButtonUp(0))
                {
                    EndMove(mousePosition);

                    mouseMoveAllowed = false;
                }
            }

        }

        AddForceToRidgidbody();
        
        // If note want's to be deleted and is close to its pool, delete it
        if (toBeDeleted && Mathf.Abs(Vector2.Distance(originalPos, transform.position)) < 0.1 )
        {
            Destroy(gameObject);
        }
    }

    // To be executed when entering a moving state by mouse or touch
    private void BeginMove(bool touchInput)
    {
        if (touchInput)
        {
            moveAllowed = true;
        } else
        {
            mouseMoveAllowed = true;
        }

        // Make sprite visible
        sprite.enabled = true;
    }

    // To be executed when actively moving by mouse or touch
    private void OnMove(Vector2 position)
    {
        transform.position = position;

        // Update the style of the helper lines
        UpdateHelperLineColor(transform.position);
    }

    // To be executed when exiting a moving state by mouse or touch
    private void EndMove(Vector2 position)
    {
        SnapToPoint(position);

        // Reset collider size
        GetComponent<CircleCollider2D>().radius = originalColliderRadius;

        // Clear helper lines
        HideAllHelperLines();
    }

    // -------------------- End of note movement --------------------



    // Method for snapping to object to a point close to it
    private void SnapToPoint(Vector2 position)
    {
        // If the snap point array is empty don't run this method
        if (snapPoints.Length == 0) return; 

        bool snapped = false;
        
        // Start values for shortest snap point
        GameObject shortestSnapPoint = snapPoints[0];
        float shortestSnapPointDistance = float.MaxValue;

        // Loops through all snap points and keeps track of the snap point closest to the object.
        foreach (GameObject snapPoint in snapPoints)
        {
            Vector2 snapPos = snapPoint.transform.position;
            // Calculates the distance from the object to each snap point
            float distToSnapPos = Vector2.Distance(transform.position, snapPoint.transform.position);
            
            // If we found a snappoint with smaller distance update shortest snap point
            if (distToSnapPos < shortestSnapPointDistance)
            {
                shortestSnapPoint = snapPoint;
                shortestSnapPointDistance = distToSnapPos;
            }

        }

        // True if you try to put down a pickedup note at the same location
        bool shortestSnapPointIsCurrent = shortestSnapPoint.transform.Equals(transform.parent);

        // True if there already is a note at the snappoint
        bool alreadyNoteInShortestSnapPoint = shortestSnapPoint.transform.childCount <= 1;

        // If the object is close to the snapPoint and the snappoint is empty of notes, set the objects position to that point
        if (shortestSnapPointDistance < 0.5f && !snapped && (shortestSnapPointIsCurrent || alreadyNoteInShortestSnapPoint))
        {
            targetPosition = shortestSnapPoint.transform.position;
            transform.SetParent(shortestSnapPoint.transform);
            
            snapped = true;

            // Sets the correct audio clip depending on which note the object was placed on
            SetCorrectNote(shortestSnapPoint);

            // Play the assigned note
            audioSource.Play();

            // Reset collider size if note has left the pool
            if (originalPos != targetPosition)
            {
                GetComponent<CircleCollider2D>().radius = originalColliderRadius;
            }
        }


        // If we did not snap to anything, return the object to the original position
        if (!snapped)
        {
            transform.SetParent(originalParent);
            transform.position = position; // This is to have the right coordinates
            targetPosition = originalPos;

            // Set the object to be deleted if it's released outside a snappoint
            toBeDeleted = true;
        }
    }

    // Adds force to the player ridgidbody to move towards the target point
    private void AddForceToRidgidbody()
    {
        if (!mouseMoveAllowed && !moveAllowed)
        {
            Vector2 forceVector = (targetPosition - transform.position) * 15 * (Time.deltaTime * 1000);
            ridgidBody.AddForce(forceVector);
        }
    }

    // Initializes snap points array
    private GameObject[] FillArrayFromParent(GameObject parent)
    {
        // Get number of children
        int nrChildren = parent.transform.childCount;
        GameObject[] children = new GameObject[nrChildren];

        // Add children to array
        for (int i = 0; i < nrChildren; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            children[i] = child;
        }

        return children;
    }

    // Changes audio clip source to match the current note the object is attached to 
    private void SetCorrectNote(GameObject snapPoint)
    {
        // Get the note portion of the objects name
        string snapPointNote = snapPoint.name.Split("-")[2];

        // Get the correct audio clip based on which note to play
        AudioClip audioClip = GetAudioClip(snapPointNote);

        // If a clip is found update audio source with new clip
        if (audioClip != null) audioSource.clip = audioClip;
    }

    // Gets correct note based on which instrument the object is
    private AudioClip GetAudioClip(string noteName)
    {
        return FindAudioClipNote(instrumentClips, noteName);
    }

    // Finds a specific note in an audio clip array
    private AudioClip FindAudioClipNote(AudioClip[] clips, string noteName)
    {
        foreach (AudioClip clip in clips)
        {
            // If we found a note with matching name, return it
            if (clip.name == noteName) return clip;
        }
        return null;
    }

    // Locks the note so it can't be moved
    public void LockNode()
    {
        lockedInPlace = true;
    }

    // Unlocks the note so it can be moved
    public void UnlockNode()
    {
        lockedInPlace = false;
    }
    public void SnapToOriginalPosAndDelete()
    {
        // Sets position to the original position
        transform.SetParent(originalParent);
        targetPosition = originalPos;

        // Set the object to be deleted when it reaches the pool
        toBeDeleted = true;
    }

    // Updates the color on all helper lines depending on the notes distance to the line
    private void UpdateHelperLineColor(Vector2 notePos)
    {
        foreach (GameObject line in helperLines)
        {
            // Get distance from note to line
            float distToNote = Vector2.Distance(line.transform.position, notePos);

            // Get sprite component
            SpriteRenderer spriteRenderer = line.GetComponent<SpriteRenderer>();

            if (distToNote < helperLineCutoffDisctance)
            {
                // Change the alpha color the percent distance from note to line. Ex 0.001 = Far away, 1 = Closest it can be 
                Color color = spriteRenderer.color;
                color.a = 1 - (distToNote / helperLineCutoffDisctance);
                spriteRenderer.color = color;
            } else
            {
                // If line too far away, make fully transparent
                spriteRenderer.color = new(0, 0, 0, 0);
            }
        }
    }

    // Hides all helper lines
    private void HideAllHelperLines()
    {
        foreach (GameObject line in helperLines)
        {
            // Make line fully transparent
            line.GetComponent<SpriteRenderer>().color = new(0, 0, 0, 0);
        }
    }


    // ------------- Setters ---------------

    // Sets the snap point parent object and initializes the array
    public void SetSnapPointParent(GameObject parent)
    {
        snapPointsParent = parent;
        snapPoints = FillArrayFromParent(parent);
    }

    // Sets the extra line parent object and initializes the array
    public void SetExtraLineParent(GameObject parent)
    {
        helperLinesParent = parent;
        helperLines = FillArrayFromParent(parent);
        HideAllHelperLines();
    }
}
