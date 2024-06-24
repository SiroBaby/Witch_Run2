using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterDatabase characterDB; // Database chứa danh sách các nhân vật

    public SpriteRenderer artworkSprite; // SpriteRenderer để hiển thị hình ảnh nhân vật

    private int selectedOption = 0; // Lựa chọn nhân vật hiện tại


    // Các biến công khai cho thuộc tính và trạng thái của người chơi
    public float gravity; // Lực hấp dẫn tác động lên người chơi
    public Vector2 velocity; // Vận tốc hiện tại của người chơi
    public float maxXVelocity = 100; // Vận tốc ngang tối đa
    public float maxAcceleration = 10; // Gia tốc tối đa
    public float acceleration = 10; // Gia tốc hiện tại
    public float distance = 0; // Khoảng cách đã di chuyển
    public float jumpVelocity = 20; // Vận tốc khi nhảy
    public float groundHeight = 10; // Chiều cao của mặt đất
    public bool isGrounded = false; // Người chơi có đang trên mặt đất không
    public bool isHoldingJump = false; // Người chơi có đang giữ phím nhảy không
    public float maxHoldJumpTime = 0.4f; // Thời gian tối đa có thể giữ phím nhảy
    public float maxMaxHoldJumpTime = 0.4f; // Thời gian tối đa có thể giữ phím nhảy (giá trị tối đa)
    public float holdJumpTimer = 0.0f; // Bộ đếm thời gian giữ phím nhảy
    public float jumpGroundThreshold = 1; // Ngưỡng để coi người chơi là đang trên mặt đất
    public bool isDead = false;

    // Start được gọi trước khi khung hình đầu tiên được cập nhật
    void Start()
    {
        // Mã khởi tạo có thể đặt ở đây

        // Kiểm tra xem đã có lựa chọn nhân vật được lưu hay chưa
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0; // Nếu chưa, lựa chọn mặc định là 0
        }
        else
        {
            Load(); // Nếu đã có, tải lựa chọn từ PlayerPrefs
        }

        UpdateCharacer(selectedOption); // Cập nhật hình ảnh cho nhân vật được chọn ban đầu
    }

    // Update được gọi một lần mỗi khung hình
    void Update()
    {
        // Lấy vị trí hiện tại của người chơi
        Vector2 pos = transform.position;
        // Tính khoảng cách từ người chơi tới mặt đất
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        // Kiểm tra nếu người chơi đang trên mặt đất hoặc trong ngưỡng nhảy
        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            // Nếu phím Space được nhấn, bắt đầu nhảy
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false; // Người chơi không còn trên mặt đất
                velocity.y = jumpVelocity; // Đặt vận tốc dọc cho nhảy
                isHoldingJump = true; // Bắt đầu giữ phím nhảy
                holdJumpTimer = 0; // Đặt lại bộ đếm thời gian giữ phím nhảy
            }
        }

        // Nếu phím Space được thả ra, dừng giữ phím nhảy
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
    }

    // FixedUpdate được gọi ở khoảng thời gian cố định và được dùng cho tính toán vật lý
    private void FixedUpdate()
    {
        // Lấy vị trí hiện tại của người chơi
        Vector2 pos = transform.position;

        if (isDead)
        {
            return;

        }

        if (pos.y < -20)
        {
            isDead = true;
        }

        // Nếu người chơi không đang trên mặt đất
        if (!isGrounded)
        {
            // Nếu đang giữ phím nhảy, tăng bộ đếm thời gian giữ phím nhảy
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                // Nếu bộ đếm thời gian giữ phím nhảy vượt quá thời gian tối đa, dừng giữ phím nhảy
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            // Cập nhật vị trí dọc của người chơi dựa trên vận tốc
            pos.y += velocity.y * Time.fixedDeltaTime;
            // Nếu không giữ phím nhảy, áp dụng lực hấp dẫn
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            // Thực hiện raycast để phát hiện va chạm với mặt đất
            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            // Nếu raycast chạm vào một collider
            if (hit2D.collider != null)
            {
                // Kiểm tra nếu collider là đối tượng mặt đất
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    // Đặt chiều cao mặt đất, vị trí của người chơi, và đặt lại vận tốc dọc
                    if (pos.y >= ground.groundHeight)
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        velocity.y = 0;
                        isGrounded = true; // Người chơi bây giờ đang trên mặt đất
                    }
                }
            }
            // Vẽ tia ray debug màu xanh lá cây
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.green);

            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;
                    }
                }
            }
        }

        // Cập nhật khoảng cách đã di chuyển của người chơi
        distance += velocity.x * Time.fixedDeltaTime;

        // Nếu người chơi đang trên mặt đất
        if (isGrounded)
        {
            // Tính tỷ lệ của vận tốc hiện tại với vận tốc tối đa
            float velocityRatio = velocity.x / maxXVelocity;
            // Điều chỉnh gia tốc dựa trên tỷ lệ vận tốc
            acceleration = maxAcceleration * (1 - velocityRatio);
            // Điều chỉnh thời gian giữ phím nhảy tối đa dựa trên tỷ lệ vận tốc
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            // Cập nhật vận tốc ngang của người chơi
            velocity.x += acceleration * Time.fixedDeltaTime;
            // Giới hạn vận tốc đến vận tốc tối đa
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            // Thực hiện raycast để kiểm tra nếu người chơi vẫn còn trên mặt đất
            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            // Nếu raycast không chạm vào bất kỳ collider nào, người chơi không còn trên mặt đất
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            // Vẽ tia ray debug màu vàng
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        }


        // Xử lý trục x của nhân vật
        Vector2 obstOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obsHitx = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime);
        if (obsHitx.collider != null)
        {
            ObstacleWater obstacleWater = obsHitx.collider.GetComponent<ObstacleWater>();
            ObstacleEnemy obstacleEnemy = obsHitx.collider.GetComponent<ObstacleEnemy>();
            if (obstacleWater != null)
            {
                hitWater(obstacleWater);
            }

            if (obstacleEnemy != null)
            {
                hitEnemy(obstacleEnemy);
            }

        }

        // Xử lý trục y của nhân vật
        RaycastHit2D obsHity = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime);
        if (obsHity.collider != null)
        {
            ObstacleWater obstacleWater = obsHity.collider.GetComponent<ObstacleWater>();
            if (obstacleWater != null)
            {
                hitWater(obstacleWater);
            }
        }

        // Cập nhật vị trí của người chơi
        transform.position = pos;
    }


    // Xử lý sự kiện khi đụng vào Quái
    void hitEnemy(ObstacleEnemy obstacleEnemy)
    {
        isDead = true;
    }

    // Xử lý sự kiện khi di chuyển vào nước
    void hitWater(ObstacleWater obstacleWater)
    {
        velocity.x = 20f;
    }

    // Phương thức cập nhật hình ảnh của nhân vật
    private void UpdateCharacer(int selectedOption)
    {
        // Lấy thông tin nhân vật từ database
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite; // Thiết lập hình ảnh cho SpriteRenderer
    }

    // Phương thức tải lựa chọn nhân vật từ PlayerPrefs
    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

}
