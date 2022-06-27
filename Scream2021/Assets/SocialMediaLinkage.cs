using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialMediaLinkage : MonoBehaviour
{
    const string twitterURL = "https://twitter.com/ExtracosmicS?ref_src=twsrc%5Etfw";

    public void FollowTwitterLink()
    {
        Application.OpenURL(twitterURL);
    }
}
