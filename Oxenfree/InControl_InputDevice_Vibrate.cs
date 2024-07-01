using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace InControl
{
	// Token: 0x020001FD RID: 509
	public partial class InputDevice
	{
		#region PInvoke for GameControllerWrapper and CoreHapticsWrapper DLLs

		[StructLayout(LayoutKind.Sequential)]
		private struct GCControllerHandle
		{
			public string UniqueId;
			public string ProductCategory;
			public string VendorName;
			[MarshalAs(UnmanagedType.I1)]
			public bool IsAttachedToDevice;
			[MarshalAs(UnmanagedType.I1)]
			public bool HasHaptics;
			[MarshalAs(UnmanagedType.I1)]
			public bool HasLight;
			[MarshalAs(UnmanagedType.I1)]
			public bool HasBattery;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct GCGetConnectedControllersResponse
		{
			public IntPtr ControllersPtr;
			public int ControllersCount;

			public GCControllerHandle[] GetControllers() {
				var results = new GCControllerHandle[ControllersCount];
				var size = Marshal.SizeOf<GCControllerHandle>();

				for (var i = 0; i < ControllersCount; i++) {
					var elementPtr = new IntPtr((long)ControllersPtr + (i * size));
					results[i] = Marshal.PtrToStructure<GCControllerHandle>(elementPtr);
				}

				return results;
			}
		}

		private const string GameControllerFrameworkWrapper = "GameControllerWrapper";
	        private const string CoreHapticsFrameworkWrapper = "CoreHapticsWrapper";
	
	        [DllImport(GameControllerFrameworkWrapper, CallingConvention=CallingConvention.Cdecl)]
	        private static extern GCGetConnectedControllersResponse GameControllerWrapper_GetConnectedControllers();
	
		private static GCControllerHandle? GetController()
		{
			// This method assumes that the first controller that supports haptics is the current controller
			var controllers = GameControllerWrapper_GetConnectedControllers().GetControllers();
			foreach (var controller in controllers) {
				if (controller.HasHaptics)
					return controller;
			}

			return null;
		}

	        [DllImport(GameControllerFrameworkWrapper, CallingConvention=CallingConvention.Cdecl)]
	        private static extern IntPtr GameControllerWrapper_CreateHapticsEngine(string uniqueId, IntPtr onError);

		[DllImport(CoreHapticsFrameworkWrapper, CallingConvention=CallingConvention.Cdecl)]
		private static extern void CoreHaptics_CHHapticEngine_Set_IsAutoShutdownEnabled(IntPtr enginePtr, bool isAutoShutdownEnabled);

		[DllImport(CoreHapticsFrameworkWrapper, CallingConvention=CallingConvention.Cdecl)]
		private static extern void CoreHaptics_CHHapticEngine_Set_PlaysHapticsOnly(IntPtr enginePtr, bool playsHapticsOnly);

	        [DllImport(CoreHapticsFrameworkWrapper, CallingConvention=CallingConvention.Cdecl)]
	        private static extern void CoreHaptics_CHHapticEngine_Start(IntPtr enginePtr, IntPtr onError);

		[DllImport(CoreHapticsFrameworkWrapper, CallingConvention=CallingConvention.Cdecl)]
		private static extern void CoreHaptics_CHHapticEngine_PlayPatternFromJson(IntPtr enginePtr, string ahapJson, IntPtr onError);

		#endregion

	        private const string HapticRumbleJSONTemplate = @"{
	            ""Version"": 1,
	            ""Pattern"": [
	                {
	                    ""Event"": {
	                        ""EventType"": ""HapticTransient"",
	                        ""EventParameters"": [
	                            {
	                                ""ParameterID"": ""HapticIntensity"",
	                                ""ParameterValue"": {intensity}
	                            }
	                        ],
	                        ""Time"": 0,
	                    }
	                }
	            ]
	        }";

		private GCControllerHandle? controller;
		private IntPtr enginePtr;

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00048E09 File Offset: 0x00047009
		public virtual void Vibrate(float leftMotor, float rightMotor)
		{
			var intensity = Math.Min((leftMotor + rightMotor), 1.0f);
			if (intensity == 0.0f)
				return;

			if (!this.controller.HasValue) {
				this.controller = GetController();
				if (this.controller == null) {
					return;
				}

				// Create haptics engine
				this.enginePtr = GameControllerWrapper_CreateHapticsEngine(this.controller?.UniqueId, IntPtr.Zero);
				if (this.enginePtr == IntPtr.Zero) {
					this.controller = null;
					return;
				}

				// Set haptics some settings
				CoreHaptics_CHHapticEngine_Set_IsAutoShutdownEnabled(this.enginePtr, false);
				CoreHaptics_CHHapticEngine_Set_PlaysHapticsOnly(this.enginePtr, true);

				// Start haptics engine
				CoreHaptics_CHHapticEngine_Start(this.enginePtr, IntPtr.Zero);
			}

			// Play haptics
			var HapticRumbleJSON = HapticRumbleJSONTemplate.Replace("{intensity}", intensity.ToString("0.000000"));
			CoreHaptics_CHHapticEngine_PlayPatternFromJson(this.enginePtr, HapticRumbleJSON, IntPtr.Zero);
		}
	}
}
