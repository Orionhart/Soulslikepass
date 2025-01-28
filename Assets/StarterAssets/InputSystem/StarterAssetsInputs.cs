using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool pause;
		public bool jump;
		public bool sprint;
		public bool action;
		public bool test;
		public bool dash;
		public bool lightAttack;
		public bool heavyAttack;


		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		public bool hideCursor = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if (PlayerScript.isPaused) return;
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (PlayerScript.isPaused) return;
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (PlayerScript.isPaused) return;
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (PlayerScript.isPaused) return;
			SprintInput(value.isPressed);
		}
		public void OnDash(InputValue value)
		{
			if (PlayerScript.isPaused) return;
			DashInput(value.isPressed);
		}
		public void OnAction(InputValue value)
		{
			if (PlayerScript.isPaused) return;
			ActionInput(value.isPressed);
		}
		public void OnLightAttack(InputValue value)
		{
			if (PlayerScript.isPaused) return;
			Debug.Log("OnLightAttack " + value);
			LightAttackInput(value.isPressed);
		}
		public void OnHeavyAttack(InputValue value)
		{
			if (PlayerScript.isPaused) return;
			Debug.Log("OnHeavyAttack " + value);
			HeavyAttackInput(value.isPressed);
		}

		public void OnPause(InputValue value)
		{
			Debug.Log("OnPaused " + value);
			PauseInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void DashInput(bool newDashState)
		{
			dash = newDashState;
		}
		public void ActionInput(bool newActionState)
		{
			action = newActionState;
		}

		public void LightAttackInput(bool newActionState)
		{
			lightAttack = newActionState;
		}
		public void HeavyAttackInput(bool newActionState)
		{
			heavyAttack = newActionState;
		}

		public void PauseInput(bool newActionState)
		{
			pause = newActionState;
		}

		private void SetCursorState(bool newState)
		{
			if (PlayerScript.isPaused) return;
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !hideCursor;
		}
	}

}