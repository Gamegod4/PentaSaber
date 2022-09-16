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
        public int SaberAState => LeftTriggerActive;
        public saberControllerObject saberACObject = new saberControllerObject(false);

        public int SaberBState => RightTriggerActive;
        public saberControllerObject saberBCObject = new saberControllerObject(true);
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
                default:
                    break;
            }
            return false;//default
        }

        //button selected numbers
        //0 = trigger
        //1 = grip
        //2 = button A
        //3 = button B

        public int RightTriggerActive
        {
            get
            {
                InputDevice device = RightController;
                saberBCObject.device = device;
                return getTriggeredState(saberBCObject);
            }
        }

        public int LeftTriggerActive
        {
            get
            {
                InputDevice device = LeftController;
                saberACObject.device = device;
                return getTriggeredState(saberACObject);
            }
        }

        private int getTriggeredState(saberControllerObject givenSCO)
        {
            bool active = false;

            bool secondaryTriggered = false;
            bool tertiaryTriggered = false;
            int stateToReturn = 0;

            if (PluginConfig.Instance.toggleLockEnabled)
            {
                if (PluginConfig.Instance.SeptaEnabled)
                {
                    if (givenSCO.device.isValid)
                    {
                        secondaryTriggered = buttonSwitchCases(givenSCO.secondaryButton, givenSCO.device, givenSCO.secondaryThreshold);
                        tertiaryTriggered = buttonSwitchCases(givenSCO.tertiaryButton, givenSCO.device, givenSCO.tertiaryThreshold);
                        active = secondaryTriggered || tertiaryTriggered;
                    }

                    if (active && givenSCO.trigPrev == false)//spam lock capture active
                    {
                        givenSCO.trigPrev = true;
                        if (secondaryTriggered)
                        {
                            if (givenSCO.saberToggleBoolT)
                            {
                                givenSCO.saberToggleBoolT = false;
                                givenSCO.saberToggleBoolS = true;
                            }
                            else if (givenSCO.saberToggleBoolS)
                            {
                                givenSCO.saberToggleBoolS = false;
                            }
                            else if (givenSCO.saberToggleBoolS == false)
                            {
                                givenSCO.saberToggleBoolS = true;
                            }
                        }
                        else if (tertiaryTriggered)
                        {
                            if (givenSCO.saberToggleBoolS)
                            {
                                givenSCO.saberToggleBoolS = false;
                                givenSCO.saberToggleBoolT = true;
                            }
                            else if (givenSCO.saberToggleBoolT)
                            {
                                givenSCO.saberToggleBoolT = false;
                            }
                            else if (givenSCO.saberToggleBoolT == false)
                            {
                                givenSCO.saberToggleBoolT = true;
                            }
                        }
                    }

                    if (active == false && givenSCO.trigPrev == true)//spam lock complete capture
                    {
                        givenSCO.trigPrev = false;
                    }

                    //set color
                    if (givenSCO.saberToggleBoolS)
                    {
                        stateToReturn = 1;
                    }
                    else if (givenSCO.saberToggleBoolT)
                    {
                        stateToReturn = 2;
                    }
                }
                else
                {
                    if (givenSCO.device.isValid)
                    {
                        secondaryTriggered = buttonSwitchCases(givenSCO.secondaryButton, givenSCO.device, givenSCO.secondaryThreshold);
                        active = secondaryTriggered;
                    }

                    if (active && givenSCO.trigPrev == false)//spam lock capture active
                    {
                        givenSCO.trigPrev = true;
                        if (givenSCO.saberToggleBoolS){ givenSCO.saberToggleBoolS = false; }
                        else if (givenSCO.saberToggleBoolS == false){ givenSCO.saberToggleBoolS = true; }
                    }

                    if (active == false && givenSCO.trigPrev == true)//spam lock complete capture
                    {
                        givenSCO.trigPrev = false;
                    }

                    //set color
                    if (givenSCO.saberToggleBoolS)
                    {
                        stateToReturn = 1;
                    }
                }
            }
            else
            {
                if (givenSCO.device.isValid)
                {
                    if (PluginConfig.Instance.SeptaEnabled)
                    {
                        if (buttonSwitchCases(givenSCO.secondaryButton, givenSCO.device, givenSCO.secondaryThreshold))//secondary
                        {
                            stateToReturn = 1;
                        }
                        else if (buttonSwitchCases(givenSCO.tertiaryButton, givenSCO.device, givenSCO.tertiaryThreshold))//tertiary
                        {
                            stateToReturn = 2;
                        }
                    }
                    else
                    {
                        if (buttonSwitchCases(givenSCO.secondaryButton, givenSCO.device, givenSCO.secondaryThreshold))//secondary
                        {
                            stateToReturn = 1;
                        }
                    }
                }
            }
            return stateToReturn;
        }
    }

    public class saberControllerObject
    {
        public InputDevice device;
        public bool saberToggleBoolS = false;
        public bool saberToggleBoolT = false;
        public bool trigPrev = false;
        public bool leftHand = true;
        public int secondaryButton = 0;
        public int tertiaryButton = 0;
        public float secondaryThreshold = 0;
        public float tertiaryThreshold = 0;

        public saberControllerObject(bool isRightHand)
        {
            if (isRightHand) { leftHand = false; }
            if (leftHand)
            {
                secondaryButton = PluginConfig.Instance.leftSecondaryButtonSelection;
                tertiaryButton = PluginConfig.Instance.leftTertiaryButtonSelection;
                secondaryThreshold = PluginConfig.Instance.leftSecondaryButtonThreshold;
                tertiaryThreshold = PluginConfig.Instance.leftTertiaryButtonThreshold;
            }
            else
            {
                secondaryButton = PluginConfig.Instance.rightSecondaryButtonSelection;
                tertiaryButton = PluginConfig.Instance.rightTertiaryButtonSelection;
                secondaryThreshold = PluginConfig.Instance.rightSecondaryButtonThreshold;
                tertiaryThreshold = PluginConfig.Instance.rightTertiaryButtonThreshold;
            }
        }
    }
}
