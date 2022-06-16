using System;
using System.Collections.Generic;
using System.Windows;

namespace Wpf.Ui.Controls;

//TODO: Just for fun

public enum QrCodeQuality
{
    /// <summary>
    /// Recovers 7% of data
    /// </summary>
    L,

    /// <summary>
    /// Recovers 15% of data
    /// </summary>
    M,

    /// <summary>
    /// Recovers 25% of data
    /// </summary>
    Q,

    /// <summary>
    /// Recovers 30% of data
    /// </summary>
    H
}

public enum QrCodeEncoding
{
    /// <summary>
    /// Chooses automatically.
    /// </summary>
    Auto,

    /// <summary>
    /// 7089 characters
    /// </summary>
    Numeric,

    /// <summary>
    /// 4296 characters
    /// </summary>
    Alphanumeric,

    /// <summary>
    /// 2953 characters
    /// </summary>
    Byte,

    /// <summary>
    /// 1817 characters
    /// </summary>
    Kanji
}

public enum QrCodeType
{
    Bitmap,
    Vector
}

/// <summary>
/// <see href="https://www.qrcode.com/en/about/standards.html">https://www.qrcode.com/en/about/standards.html</see>
/// </summary>
[Obsolete]
internal class QrCode : System.Windows.Controls.Control
{
    // ISO 8859-1 | ISO 8859-2 | UTF-8

    private string _rawData = String.Empty;

    /// <summary>
    /// Property for <see cref="Quality"/>.
    /// </summary>
    public static readonly DependencyProperty QualityProperty =
        DependencyProperty.Register(nameof(Quality), typeof(QrCodeQuality), typeof(QrCode),
            new PropertyMetadata(QrCodeQuality.M));

    /// <summary>
    /// Property for <see cref="Encoding"/>.
    /// </summary>
    public static readonly DependencyProperty EncodingProperty =
        DependencyProperty.Register(nameof(Encoding), typeof(QrCodeEncoding), typeof(QrCode),
            new PropertyMetadata(QrCodeEncoding.Auto));

    /// <summary>
    /// Property for <see cref="Type"/>.
    /// </summary>
    public static readonly DependencyProperty TypeProperty =
        DependencyProperty.Register(nameof(Type), typeof(QrCodeType), typeof(QrCode),
            new PropertyMetadata(QrCodeType.Bitmap));

    /// <summary>
    /// Gets or sets the QR Code error correction level.
    /// </summary>
    public QrCodeQuality Quality
    {
        get => (QrCodeQuality)GetValue(QualityProperty);
        set => SetValue(QualityProperty, value);
    }

    /// <summary>
    /// Gets or sets the QR Code encoding.
    /// </summary>
    public QrCodeEncoding Encoding
    {
        get => (QrCodeEncoding)GetValue(EncodingProperty);
        set => SetValue(EncodingProperty, value);
    }

    /// <summary>
    /// Gets or sets the QR Code generation type.
    /// </summary>
    public QrCodeType Type
    {
        get => (QrCodeType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    private static int GetModeIndicator(QrCodeEncoding encoding)
    {
        return encoding switch
        {
            QrCodeEncoding.Numeric => 0x0001,
            QrCodeEncoding.Alphanumeric => 0x0010,
            QrCodeEncoding.Byte => 0x0100,
            QrCodeEncoding.Kanji => 0x1000,
            _ => 0x0000
        };
    }

    private static byte[] Generate()
    {
        var bytes = new List<byte>();

        bytes.AddRange(new byte[] { 0x51, 0x52, 0x52, 0x00 });

        return bytes.ToArray();
    }

    private static byte[] Encode(string data, QrCodeEncoding encoding)
    {
        var bytes = new List<byte>();

        return bytes.ToArray();
    }
}
