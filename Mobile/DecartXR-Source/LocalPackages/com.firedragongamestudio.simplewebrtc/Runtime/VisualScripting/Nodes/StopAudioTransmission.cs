#if VISUAL_SCRIPTING_INSTALLED
using Unity.VisualScripting;

namespace SimpleWebRTC {
    [UnitCategory("SimpleWebRTC")]
    public class StopAudioTransmission : Unit {
        [DoNotSerialize, PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }

        [DoNotSerialize, PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        protected override void Definition() {
            InputTrigger = ControlInput("InputTrigger", SetupConnection);
            OutputTrigger = ControlOutput("OutputTrigger");

            Succession(InputTrigger, OutputTrigger);
        }

        private ControlOutput SetupConnection(Flow flow) {
            SimpleWebRTCSceneVariables.ClientConnection.StopAudioTransmission();

            return OutputTrigger;
        }
    }
}
#endif