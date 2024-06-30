using System;
using System.Collections.Generic;
using Gendarme;
using Platform.Utils;
using UnityEngine;

// Token: 0x020002DC RID: 732
public partial class GameInput : MonoBehaviour
{
	// Token: 0x0600164A RID: 5706 RVA: 0x000766FC File Offset: 0x000748FC
	private void UpdateAxisValues(bool useKeyboard, bool useController)
	{
		if (useController)
		{
			if (GameInput.GetUseOculusInputManager())
			{
				Vector2 vector = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.Active);
				GameInput.axisValues[2] = vector.x;
				GameInput.axisValues[3] = -vector.y;
				Vector2 vector2 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.Active);
				GameInput.axisValues[0] = vector2.x;
				GameInput.axisValues[1] = -vector2.y;
				GameInput.axisValues[4] = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active);
				GameInput.axisValues[5] = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger, OVRInput.Controller.Active);
				GameInput.axisValues[6] = 0f;
				if (OVRInput.Get(OVRInput.RawButton.DpadLeft, OVRInput.Controller.Active))
				{
					GameInput.axisValues[6] -= 1f;
				}
				if (OVRInput.Get(OVRInput.RawButton.DpadRight, OVRInput.Controller.Active))
				{
					GameInput.axisValues[6] += 1f;
				}
				GameInput.axisValues[7] = 0f;
				if (OVRInput.Get(OVRInput.RawButton.DpadUp, OVRInput.Controller.Active))
				{
					GameInput.axisValues[7] += 1f;
				}
				if (OVRInput.Get(OVRInput.RawButton.DpadDown, OVRInput.Controller.Active))
				{
					GameInput.axisValues[7] -= 1f;
				}
			}
			else
			{
				GameInput.ControllerLayout controllerLayout = GameInput.GetControllerLayout();
				if (controllerLayout == GameInput.ControllerLayout.Xbox360 || controllerLayout == GameInput.ControllerLayout.XboxOne || GameInput.IsPS4OrPS5Platform() || controllerLayout == GameInput.ControllerLayout.Scarlett)
				{
					GameInput.axisValues[2] = UnityEngine.Input.GetAxis("ControllerAxis1");
					GameInput.axisValues[3] = UnityEngine.Input.GetAxis("ControllerAxis2");
					GameInput.axisValues[0] = UnityEngine.Input.GetAxis("ControllerAxis3");
					GameInput.axisValues[1] = UnityEngine.Input.GetAxis("ControllerAxis4");
					if (controllerLayout == GameInput.ControllerLayout.Xbox360)
					{
						GameInput.axisValues[4] = Mathf.Max(-UnityEngine.Input.GetAxis("ControllerAxis3"), 0f);
						GameInput.axisValues[5] = Mathf.Max(UnityEngine.Input.GetAxis("ControllerAxis3"), 0f);
					}
					else if (controllerLayout == GameInput.ControllerLayout.XboxOne)
					{
						// NOTE: This patch is for XBox One Wireless Controller only
						GameInput.axisValues[4] = UnityEngine.Input.GetAxis("ControllerAxis5");
						GameInput.axisValues[5] = UnityEngine.Input.GetAxis("ControllerAxis6");

						var axis7_value = UnityEngine.Input.GetAxis("ControllerAxis7");
						var axis8_value = UnityEngine.Input.GetAxis("ControllerAxis8");

						if (axis7_value == 1 && axis8_value == 1)
						{
							// DPad Right
							GameInput.axisValues[6] = 1f;
							GameInput.axisValues[7] = 0f;
						}
						else if (axis7_value == -1 && axis8_value == 1)
						{
							// DPad Down
							GameInput.axisValues[6] = 0f;
							GameInput.axisValues[7] = -1f;
						}
						else if (axis7_value == 1 && axis8_value == -1)
						{
							// DPad Up
							GameInput.axisValues[6] = 0f;
							GameInput.axisValues[7] = 1f;
						}
						else if (axis7_value == -1 && axis8_value == -1)
						{
							// DPad Left
							GameInput.axisValues[6] = -1f;
							GameInput.axisValues[7] = 0f;
						}
						else
						{
							GameInput.axisValues[6] = 0f;
							GameInput.axisValues[7] = 0f;
						}
					}
					else if (GameInput.IsPS4OrPS5Platform())
					{
						GameInput.axisValues[4] = Mathf.Max(UnityEngine.Input.GetAxis("ControllerAxis8"), 0f);
						GameInput.axisValues[5] = Mathf.Max(-UnityEngine.Input.GetAxis("ControllerAxis3"), 0f);
					}
					else if (controllerLayout == GameInput.ControllerLayout.Scarlett)
					{
						GameInput.axisValues[4] = Mathf.Max(UnityEngine.Input.GetAxis("ControllerAxis9"), 0f);
						GameInput.axisValues[5] = Mathf.Max(UnityEngine.Input.GetAxis("ControllerAxis10"), 0f);
					}
				}
				else if (controllerLayout == GameInput.ControllerLayout.Switch)
				{
					GameInput.axisValues[2] = InputUtils.GetAxis("ControllerAxis1");
					GameInput.axisValues[3] = InputUtils.GetAxis("ControllerAxis2");
					GameInput.axisValues[0] = InputUtils.GetAxis("ControllerAxis4");
					GameInput.axisValues[1] = InputUtils.GetAxis("ControllerAxis5");
					GameInput.axisValues[4] = Mathf.Max(InputUtils.GetAxis("ControllerAxis3"), 0f);
					GameInput.axisValues[5] = Mathf.Max(-InputUtils.GetAxis("ControllerAxis3"), 0f);
					GameInput.axisValues[6] = InputUtils.GetAxis("ControllerAxis6");
					GameInput.axisValues[7] = InputUtils.GetAxis("ControllerAxis7");
				}
				else if (controllerLayout == GameInput.ControllerLayout.PS4)
				{
					GameInput.axisValues[2] = UnityEngine.Input.GetAxis("ControllerAxis1");
					GameInput.axisValues[3] = UnityEngine.Input.GetAxis("ControllerAxis2");
					GameInput.axisValues[0] = UnityEngine.Input.GetAxis("ControllerAxis3");
					GameInput.axisValues[1] = UnityEngine.Input.GetAxis("ControllerAxis6");
					GameInput.axisValues[4] = (UnityEngine.Input.GetAxis("ControllerAxis4") + 1f) * 0.5f;
					GameInput.axisValues[5] = (UnityEngine.Input.GetAxis("ControllerAxis5") + 1f) * 0.5f;
					GameInput.axisValues[6] = UnityEngine.Input.GetAxis("ControllerAxis7");
					GameInput.axisValues[7] = UnityEngine.Input.GetAxis("ControllerAxis8");
				}
			}
		}
		if (useKeyboard)
		{
			GameInput.axisValues[10] = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
			GameInput.axisValues[8] = UnityEngine.Input.GetAxisRaw("Mouse X");
			GameInput.axisValues[9] = UnityEngine.Input.GetAxisRaw("Mouse Y");
		}
		for (int j = 0; j < GameInput.axisValues.Length; j++)
		{
			GameInput.AnalogAxis axis = (GameInput.AnalogAxis)j;
			GameInput.Device deviceForAxis = this.GetDeviceForAxis(axis);
			float f = GameInput.lastAxisValues[j] - GameInput.axisValues[j];
			GameInput.lastAxisValues[j] = GameInput.axisValues[j];
			if (deviceForAxis != GameInput.lastDevice)
			{
				float num = 0.1f;
				if (Mathf.Abs(f) > num)
				{
					GameInput.lastDevice = deviceForAxis;
				}
				else
				{
					GameInput.axisValues[j] = 0f;
				}
			}
		}
	}
}
