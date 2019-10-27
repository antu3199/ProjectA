using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSequence : MonoBehaviour {

  public List<Image> arrows;
  public List<Sprite> arrowImages;
  public void doSequenceAnimation(SwipeDirection dir, bool anim) {
    arrows[0].sprite = arrowImages[(int)dir - 1];
  }
}
