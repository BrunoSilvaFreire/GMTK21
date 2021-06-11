namespace GMTK.Cinematics.Dialogue {
    public class GlobalDialoguePanel : DialoguePanel {
        private static GlobalDialoguePanel instance;

        public static GlobalDialoguePanel Instance =>
            instance ? instance : instance = FindObjectOfType<GlobalDialoguePanel>();
    }
}