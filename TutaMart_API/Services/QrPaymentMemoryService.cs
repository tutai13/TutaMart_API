using Microsoft.Extensions.Caching.Memory;

public class QrPaymentMemoryService
{
    private readonly IMemoryCache _cache;
    private const string CacheKeyPrefix = "qr_paid_order:";

    public QrPaymentMemoryService(IMemoryCache cache)
    {
        _cache = cache;
    }

    // Webhook gọi khi thành công
    public void MarkOrderAsPaid(string referenceCode)
    {
        string key = CacheKeyPrefix + referenceCode;
        _cache.Set(key, true, TimeSpan.FromMinutes(5)); 
        Console.WriteLine($"MemoryCache: Đánh dấu {referenceCode} đã thanh toán");
    }

    // Frontend polling gọi
    public bool IsOrderPaid(string referenceCode)
    {
        string key = CacheKeyPrefix + referenceCode;
        if (_cache.TryGetValue(key, out bool paid) && paid)
        {
            _cache.Remove(key); 
            return true;
        }
        return false;
    }
}