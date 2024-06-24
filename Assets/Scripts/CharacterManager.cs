using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Sử dụng UI của Unity
using UnityEngine.SceneManagement; // Sử dụng SceneManager của Unity để chuyển scene

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB; // Database chứa danh sách các nhân vật
    public SpriteRenderer artworkSprite; // SpriteRenderer để hiển thị hình ảnh nhân vật
    public Animator characterAnimator; // Animator để điều khiển animation của nhân vật

    private int selectedOption = 0; // Lựa chọn nhân vật hiện tại

    void Start()
    {
        // Kiểm tra xem đã có lựa chọn nhân vật được lưu hay chưa
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0; // Nếu chưa, lựa chọn mặc định là 0
        }
        else
        {
            Load(); // Nếu đã có, tải lựa chọn từ PlayerPrefs
        }

        UpdateCharacter(selectedOption); // Cập nhật hình ảnh và animation cho nhân vật được chọn ban đầu
    }

    // Phương thức chuyển sang nhân vật kế tiếp
    public void NextOption()
    {
        selectedOption++; // Tăng lựa chọn lên

        // Nếu đã đến nhân vật cuối cùng, quay lại nhân vật đầu tiên
        if (selectedOption >= characterDB.CharacterCount)
        {
            selectedOption = 0;
        }

        UpdateCharacter(selectedOption); // Cập nhật hình ảnh và animation cho nhân vật mới được chọn
        Save(); // Lưu lại lựa chọn mới
    }

    // Phương thức quay lại nhân vật trước đó
    public void BackOption()
    {
        selectedOption--; // Giảm lựa chọn xuống

        // Nếu đã đến nhân vật đầu tiên, chuyển đến nhân vật cuối cùng
        if (selectedOption < 0)
        {
            selectedOption = characterDB.CharacterCount - 1;
        }

        UpdateCharacter(selectedOption); // Cập nhật hình ảnh và animation cho nhân vật mới được chọn
        Save(); // Lưu lại lựa chọn mới
    }

    // Phương thức cập nhật hình ảnh và animation của nhân vật
    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption); // Lấy thông tin nhân vật từ database
        artworkSprite.sprite = character.characterSprite; // Thiết lập hình ảnh cho SpriteRenderer

        // Update Animator Controller
        characterAnimator.runtimeAnimatorController = character.animatorController;
    }

    // Phương thức tải lựa chọn nhân vật từ PlayerPrefs
    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    // Phương thức lưu lựa chọn nhân vật vào PlayerPrefs
    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }

    // Phương thức chuyển đổi scene với sceneID được truyền vào
    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
