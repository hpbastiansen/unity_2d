using UnityEngine;

/// This script is responsible for giving the player tokens when its methods are called.
public class GiveTokens : MonoBehaviour
{
    public TokenManager TokenManagerScript;

    /// Called before the first frame.
    void Start()
    {
        TokenManagerScript = FindObjectOfType<TokenManager>();
    }

    /// Add the cactus token to the player.
    public void GiveCactusToken()
    {
        TokenManagerScript.AddTokens(TokenManagerScript.CactusToken);
    }

    /// Add the revolver token to the player.
    public void GiveRevolverToken()
    {
        TokenManagerScript.AddTokens(TokenManagerScript.RevloverToken);
    }

    /// Add the worm token to the player.
    public void GiveWormToken()
    {
        TokenManagerScript.AddTokens(TokenManagerScript.WormToken);
    }
}
