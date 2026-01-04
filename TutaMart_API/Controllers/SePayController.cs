using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/sepay")]
public class SePayController : ControllerBase
{
    private readonly string _apiKey = "YOUR_API_KEY_BAN_TAO_O_DASHBOARD";
    private readonly QrPaymentMemoryService _qrService;

    public SePayController(QrPaymentMemoryService qrService)
    {
        _qrService = qrService;
    }
    [HttpPost("webhook")]
    public async Task<IActionResult> ReceiveWebhook([FromServices] QrPaymentMemoryService qrService)
    {
        // Đọc payload
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        string payload = await reader.ReadToEndAsync();

        // Kiểm tra API Key (nếu có)
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(_apiKey))
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Apikey "))
                return Unauthorized("Missing API Key");

            string receivedKey = authHeader.Substring("Apikey ".Length).Trim();
            if (receivedKey != _apiKey)
                return Unauthorized("Invalid API Key");
        }

        try
        {
            var transaction = JsonSerializer.Deserialize<SePayWebhookData>(payload,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (transaction == null || transaction.TransferType != "in")
                return Ok();

            string content = transaction.Content ?? "";
            string referenceCode = ExtractReferenceCode(content);

            if (!string.IsNullOrEmpty(referenceCode))
            {
                Console.WriteLine($"[WEBHOOK] Mã đơn tìm thấy: {referenceCode}");

                // GỌI HÀM SERVICE ĐÃ CÓ SẴN
                qrService.MarkOrderAsPaid(referenceCode);

                Console.WriteLine($"[WEBHOOK] Đã đánh dấu thanh toán cho mã: {referenceCode}");
            }
            else
            {
                Console.WriteLine("[WEBHOOK] Không tìm thấy mã đơn hợp lệ trong nội dung chuyển khoản");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WEBHOOK] Lỗi xử lý: {ex.Message}");
        }

        return Ok();
    }

    // Hàm trích mã đơn (DH + số)
    private string ExtractReferenceCode(string content)
    {
        if (string.IsNullOrEmpty(content)) return null;

        var match = System.Text.RegularExpressions.Regex.Match(content, @"DH\d+", RegexOptions.IgnoreCase);
        return match.Success ? match.Value.ToUpper() : null;
    }
    [HttpGet("qr/check/{referenceCode}")]
    public ActionResult<bool> CheckQRPayment(string referenceCode)
    {
        bool paid = _qrService.IsOrderPaid(referenceCode);
        return Ok(paid);
    }
}

// Model dữ liệu (giữ nguyên)
public class SePayWebhookData
{
    public long Id { get; set; }
    public string Gateway { get; set; } = "";
    public string TransactionDate { get; set; } = "";
    public string AccountNumber { get; set; } = "";
    public string Content { get; set; } = "";
    public string TransferType { get; set; } = "";
    public decimal TransferAmount { get; set; }
    public decimal Accumulated { get; set; }
    public string? ReferenceCode { get; set; }
    public string? Description { get; set; }
}