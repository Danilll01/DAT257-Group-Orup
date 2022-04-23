using UnityEngine;
using System.Collections;

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
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		IAstarAI ai;

		private bool canClick = true;
		[SerializeField] private Camera mainCam;

		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
			
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update() {

			if (Input.GetMouseButtonDown(0) && canClick) { 
				target.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if (target != null && ai != null) ai.destination = target.position;
			}
		}

		public void canGetNewPos(bool input) {
			canClick = input;
        }

		public void setNewGoToPosition(int whatCollider) { 

			Vector2 runTo = Vector2.zero;

			switch (whatCollider) {
				case 1:
					runTo = mainCam.ScreenToWorldPoint(new Vector3(-50, mainCam.pixelHeight / 2));
					break;
				case 2:
					runTo = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth / 2, mainCam.pixelHeight + 50));
					break;
				case 3:
					runTo = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth + 50, mainCam.pixelHeight / 2));
					break;
				case 4:
					runTo = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth / 2, -50));
					break;
				default:
					Debug.Log("This should never happen! If you see this report it!!");
					break;
			}

			setNewPath(runTo);
		}

		private void setNewPath(Vector2 newPosVector) {
			target.position = newPosVector;
			if (target != null && ai != null) ai.destination = target.position;
		}
	}
}
