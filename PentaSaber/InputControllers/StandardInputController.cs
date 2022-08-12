using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR;

namespace PentaSaber.InputControllers
{
    public class StandardInputController : IInputController
    {
        private InputDevice leftController; 
        private InputDevice rightController;
        private static int rightFailedTries = 0;
        private static int leftFailedTries = 0;
        public bool SaberAToggled => LeftTriggerActive;

        public bool SaberBToggled => RightTriggerActive;

        internal InputDevice LeftController
        {
            get
            {
                if (leftController.isValid)
                {
                    leftFailedTries = 0;
                    return leftController;
                }
                leftFailedTries++;
                if (leftFailedTries > 100)
                {
                    leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    leftFailedTries = 0;
                }
                return leftController;
            }
            set => leftController = value;
        }
        internal InputDevice RightController
        {
            get
            {
                if (rightController.isValid)
                {
                    rightFailedTries = 0;
                    return rightController;
                }
                rightFailedTries++;
                if (rightFailedTries > 100)
                {
                    rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                    rightFailedTries = 0;
                }
                return rightController;
            }
            set => rightController = value;
        }
        
        private bool buttonSwitchCases(int buttonSelection, InputDevice givenDevice, float givenThreshold)
        {
            switch (buttonSelection)
            {
                case 0:
                    { if (givenDevice.TryGetFeatureValue(CommonUsages.trigger, out float value)) { return value > givenThreshold; } break; }
                case 1:
                    { if (givenDevice.TryGetFeatureValue(CommonUsages.grip, out float value)) { return value > givenThreshold; } break; }
                case 2:
                    { if (givenDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool value)) { return value; } break; }
                case 3:
                    { if (givenDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool value)) { return value; } break; }
            }
            return false;//default
        }
        //button selected numbers
        //0 = trigger
        //1 = grip
        //2 = button A
        //3 = button B

        public bool RightTriggerActive
        {
            get
            {
                InputDevice device = RightController;
                bool active = false;

                if (PluginConfig.Instance.toggleLockEnabled)//there might be an issue with switching between modes that causes the prev bool to screw me
                {
                    if (device.isValid)
                    {
                        active = buttonSwitchCases(PluginConfig.Instance.rightButtonSelection, device, PluginConfig.Instance.rightButtonThreshold);
                    }
                    if (active && !rightTrigPrev)
                    {
                        saberBToggleBool = !saberBToggleBool;//toggle
                        rightTrigPrev = active;
                    }
                    if (active == false && !rightTrigPrev == false)
                    {
                        rightTrigPrev = active;//untoggle?
                    }
                    return saberBToggleBool;
                }
                else
                {
                    if (device.isValid)
                    {
                        active = buttonSwitchCases(PluginConfig.Instance.rightButtonSelection, device, PluginConfig.Instance.rightButtonThreshold);
                        return active;
                    }
                    return false;
                }
            }
        }

        public bool LeftTriggerActive
        {
            get
            {
                InputDevice device = LeftController;
                bool active = false;

                if (PluginConfig.Instance.toggleLockEnabled)//there might be an issue with switching between modes that causes the prev bool to screw me
                {
                    if (device.isValid)
                    {
                        active = buttonSwitchCases(PluginConfig.Instance.leftButtonSelection, device, PluginConfig.Instance.leftButtonThreshold);
                    }
                    if (active && !leftTrigPrev)
                    {
                        saberAToggleBool = !saberAToggleBool;//toggle
                        leftTrigPrev = active;
                    }
                    if (active == false && !leftTrigPrev == false)
                    {
                        leftTrigPrev = active;//untoggle?
                    }
                    return saberAToggleBool;
                }
                else
                {
                    if (device.isValid)
                    {
                        active = buttonSwitchCases(PluginConfig.Instance.leftButtonSelection, device, PluginConfig.Instance.leftButtonThreshold);
                        return active;
                    }
                    return false;
                }
            }
        }
    }
}
