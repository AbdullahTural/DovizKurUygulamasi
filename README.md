# Döviz Kurları Takip ve Uyarı Sistemi

Bu proje, Türkiye Cumhuriyet Merkez Bankası (TCMB) tarafından sağlanan döviz kurlarını çekerek veritabanına kaydeden ve belirlenen limitlere ulaşıldığında e-posta ile uyarı gönderen **katmanlı mimariye sahip bir sistemdir**. Sistem, hem bir **masaüstü uygulaması (Windows Forms)** hem de bir **Windows Service** bileşeninden oluşur.

## Proje Bileşenleri

1.  **Windows Hizmeti (Doviz.ApiServis)**:
    - Arka planda sürekli çalışır.
    - Belirlenen zaman aralıklarında (varsayılan: 2 dakika) TCMB'nin API'sinden güncel döviz kurlarını çeker.
    - Çekilen verileri **SQL Server** veritabanına kaydeder.
    - Tanımlanan uyarı limitlerine ulaşıldığında ilgili kullanıcılara e-posta bildirimi gönderir.

2.  **Masaüstü Uygulaması (Doviz.WinApp)**:
    - Veritabanındaki güncel kur bilgilerini kullanıcı arayüzünde görüntüler.
    - Kullanıcının kur değerlerini manuel olarak güncellemesini sağlar.

## Proje Yapısı

-   **Entities**: Projede kullanılan veri yapılarını (örneğin, `ParaBirimi.cs`, `Kur.cs`, `KurGecmis.cs`) ve JSON verilerini temsil eden sınıfları içerir.
-   **Core (Business Logic)**: İş mantığının ve veritabanı işlemlerinin yürütüldüğü katmandır. `BusinessLogicLayer.cs`, API'den veri çekme ve e-posta gönderme gibi işlevleri barındırır. `DatabaseLogicLayer.cs`, SQL Server ile olan tüm etkileşimleri yönetir.
-   **Windows Forms**: Masaüstü uygulaması için kullanıcı arayüzü ve olay işleyicilerini içerir.
-   **Windows Service**: Arka plan servisi için gerekli bileşenleri ve zamanlayıcı (Timer) mantığını içerir.
-   **SQL**: `TSQLKodlari.sql` dosyası, projenin kullandığı veritabanı yapısını (tablolar ve stored procedure'ler) oluşturmak için gerekli SQL kodlarını içerir.

## Özellikler

-   **Gerçek Zamanlı Veri Çekme**: TCMB'nin API'sinden anlık döviz kurları alınır.
-   **Veritabanı Entegrasyonu**: Güncel kur bilgileri kalıcı olarak SQL Server'a kaydedilir.
-   **Katmanlı Mimari**: Kodun sürdürülebilirliğini ve okunabilirliğini artırmak için katmanlı bir yapı (Entity, Business Logic, Data Access) kullanılmıştır.
-   **E-posta Uyarı Sistemi**: Belirlenen döviz kuru bir uyarı limitine ulaştığında, e-posta ile bildirim gönderilir.

## Kurulum ve Çalıştırma

1.  **Veritabanı Kurulumu**: `TSQLKodlari.sql` dosyasındaki kodları SQL Server'da çalıştırarak `Doviz` veritabanını, gerekli tabloları (`ParaBirimi`, `Kur`, `KurGecmis`) ve stored procedure'u (`KurKayitEKLE`) oluşturun.
2.  **Proje Ayarları**: Visual Studio'da projeyi açın ve aşağıdaki dosyaları kendi ortamınıza göre düzenleyin:
    -   `DatabaseLogicLayer.cs`: SQL Server bağlantı dizgesini (`SqlConnection`) kendi sunucu bilgilerinize göre güncelleyin.
    -   `BusinessLogicLayer.cs`: E-posta gönderme ayarlarını (`kullaniciAdi` ve `uygulamaSifresi`) kendi Gmail hesabınızın **uygulama şifresiyle** güncelleyin. Güvenlik nedeniyle, normal şifrenizi kullanmayın.
3.  **Masaüstü Uygulaması (WinApp)**: `Doviz.WinApp` projesini ana başlangıç projesi olarak ayarlayıp doğrudan çalıştırabilirsiniz.
4.  **Windows Hizmeti (ApiServis)**:
    -   Projeyi derleyin.
    -   Windows Geliştirici Komut İstemi'ni (Developer Command Prompt) yönetici olarak açın.
    -   Projenin `bin/Debug` (veya `bin/Release`) klasörüne gidin.
    -   Hizmeti kurmak için aşağıdaki komutu çalıştırın:
        ```bash
        installutil Doviz.ApiServis.exe
        ```
    -   Hizmeti başlatmak için Windows Hizmetleri (services.msc) uygulamasını açın, `DovizServisi` adındaki hizmeti bulun ve başlatın.

## Katkıda Bulunma

Geliştirmelere katkıda bulunmak isterseniz, lütfen bir pull request gönderin veya bir issue açın.