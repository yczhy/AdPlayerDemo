using UnityEngine;

namespace Duskvern
{
    public static class SpriteRenderUtils
    {
        public enum SpriteRenderSortNum
        {
            BG = 0,

            // 1 - 100 for entities

            // 101 - 200 for UI

            // 201 - 300 for effects
        }

        public static int SetSpriteRenderSort(this SpriteRenderer spriteRenderer, SpriteRenderSortNum sortNum)
        {
            int originSortNum = spriteRenderer.sortingOrder;
            spriteRenderer.sortingOrder = (int)sortNum;
            return originSortNum;
        }

        public static int SetSpriteRenderSort(this SpriteRenderer spriteRenderer, int sortNum)
        {
            int originSortNum = spriteRenderer.sortingOrder;
            spriteRenderer.sortingOrder = sortNum;
            return originSortNum;
        }

        public static void SetSpriteRender(this SpriteRenderer spriteRenderer, Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
