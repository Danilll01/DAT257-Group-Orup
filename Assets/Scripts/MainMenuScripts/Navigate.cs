using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class Navigate : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		public IAstarAI ai;

		[SerializeField] private bool canClickToSetPosition = false;
		[SerializeField] private JumpNodeScript[] jumpNodes;
		[SerializeField] private PlayerAnimatorController playerAnimator;
		private Vector3 spritePos;
		private bool isWaitingOnPortalReached = false;

		void OnEnable() {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += FixedUpdate;

			spritePos = transform.GetChild(0).transform.localPosition;

		}

		void OnDisable() {
			if (ai != null) ai.onSearchPath -= FixedUpdate;
		}

		/// <summary>Updates the AI's destination every frame</summary>

		void Update() {

			// Sets target after mouse click
			if (canClickToSetPosition && Input.GetMouseButtonDown(0)) {
				target.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if (target != null && ai != null) ai.destination = target.position;
			}

			// Checks if agent is at a jumping node 
			checkJumpNode();

			// Animate player
			if (playerAnimator != null)	playerAnimator.UpdatePlayerAnimation(ai.velocity);
		}

		void FixedUpdate() {
			// Checks if agent is at a jumping node 
			checkJumpNode();
		}

		// Checks if player agent is at a jumping node and is going to do the jump, if so it calls for the jump animation to be played
		private void checkJumpNode() {
            foreach (JumpNodeScript node in jumpNodes) {

				// If agent is at a jumping node
				if (Vector2.Distance(ai.position, node.JumpFromPosition()) < 0.1) {
					
					// Check the remaining path
					List<Vector3> remPath = new List<Vector3>();
					ai.GetRemainingPath(remPath, out bool stale);
					
					// And see if the next node is the same as the jumpnode is going to 
					if (!stale && Vector2.Distance(remPath[1], node.JumpToCoordinates()) < 0.1) {

						// If so, play jumping animation
						float normalSpeed = ai.maxSpeed;
						ai.maxSpeed = node.GetJumpSpeed();
						node.StartJumpAnimation(transform.GetChild(0), spritePos,

							// Lambda to set speed back to normal after jump
							() => {
								ai.maxSpeed = normalSpeed;
							});

					}

                }
            }
        }

		// Sets a new path for the agent to pathfind to
		public void setNewPath(Vector2 newPosVector) {
			target.position = newPosVector;
			if (target != null && ai != null) ai.destination = target.position;
		}


		// Sets new position to go to and wait for end of destination to do the given Action (Often swich scenes)
		public void MoveTowardsPortal(Vector2 newPosVector, Action callToMethod) {

			target.position = newPosVector;
			if (target != null && ai != null) {
				ai.destination = target.position;

				// Stops all coroutines (to reset them) if a new call comes in
                if (isWaitingOnPortalReached) {
					StopAllCoroutines();
                }

				// Starts waiting for when agent have reached it's destination
				isWaitingOnPortalReached = true;
				StartCoroutine(waitUntillPortalReached(callToMethod));
			}
		}

		// Calls the scene switcher method when the destination of the path is reached
		private IEnumerator waitUntillPortalReached(Action callToMethod) {

			// Wait untill the ai has reached portal
			while (!ai.reachedDestination) {
				yield return null;
			}

			playerAnimator.EnterPortal();

			// Small delay before switching
			yield return new WaitForSeconds(0.3f);

			callToMethod();

			isWaitingOnPortalReached = false;
		}

	}
}

