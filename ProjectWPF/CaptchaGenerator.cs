using System;
using System.Text;

public static class CaptchaGenerator
{
    private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateCaptcha(int length = 5)
    {
        var random = new Random();
        var captcha = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            captcha.Append(chars[random.Next(chars.Length)]);
        }

        return captcha.ToString();
    }
}
