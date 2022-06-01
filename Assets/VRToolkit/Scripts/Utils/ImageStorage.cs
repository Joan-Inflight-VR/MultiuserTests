using System.Collections.Generic;
using UnityEngine;

public static class ImageStorage
{
    private static Dictionary<string, Sprite> spriteStorage;


    /// <summary>
    /// Call this as soon as possible before using the ImageStore functionalities.
    /// </summary>
    public static void SetUp()
    {
        spriteStorage = new Dictionary<string, Sprite>();
    }

    /// <summary>
    /// Store the sprite with the identifier.
    /// </summary>
    /// <param name="identifier">Your image identifier.</param>
    /// <param name="sprite">The Sprite you want to store.</param>
    public static void StoreSprite(string identifier, Sprite sprite)
    {
        spriteStorage[identifier] = sprite;
    }

    /// <summary>
    /// Returns the Sprite stored with an specific identifier. If there is no match, it will return null.
    /// </summary>
    /// <param name="identifier">Id to retrieve the Sprite.</param>
    /// <returns>The storage Sprite</returns>
    public static Sprite GetSprite(string identifier)
    {
        if (spriteStorage.TryGetValue(identifier, out Sprite sprite))
        {
            return sprite;
        }

        return null;
    }
}
