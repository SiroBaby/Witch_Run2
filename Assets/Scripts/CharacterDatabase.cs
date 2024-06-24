using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dòng này khai báo namespace và sử dụng các namespace cần thiết
[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{
    // khai báo một mảng các đối tượng Character
    public Character[] characters;

    // định nghĩa một thuộc tính chỉ đọc trả về số lượng nhân vật trong database
    public int CharacterCount => characters.Length;

    // Phương thức trả về đối tượng Character ở vị trí index trong mảng characters
    public Character GetCharacter(int index)
    {
        return characters[index];
    }

    // Phương thức trả về RuntimeAnimatorController của nhân vật ở vị trí index trong mảng characters
    public RuntimeAnimatorController GetAnimator(int index)
    {
        return characters[index].animatorController;
    }
}
