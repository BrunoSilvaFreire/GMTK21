using GMTK.Master;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace GMTK.Editor {
    [CustomTimelineEditor(typeof(LoopRegionPlayableAsset))]
    public class LoopRegionPlayableAssetEditor : ClipEditor {
        public override void OnClipChanged(TimelineClip clip) {
            var asset = ((LoopRegionPlayableAsset) clip.asset);
            asset.loopTime = clip.start;
        }
    }
}