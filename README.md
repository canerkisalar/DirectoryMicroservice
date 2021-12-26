# .NET Microservice Örnek Rehber Uygulaması

Mikroservisler ve Docker Containerlarına dayalı .Net uygulama örneği .


## Başlarken

Docker uygulamasının kurulu ve konfigürasyonu yapıldığına eminseniz , aşağıdaki kodu çalıştırarak `Phonebook App` uygulamasını hazır hale getirebilirsiz. 

```powershell
docker-compose up --build
```

## Seneryo

Birden fazla mikroservis kullanarak basit bir rehber uygulaması geliştirilmesi.

Rehberde kişi oluşturma
Rehberde kişi kaldırma
Rehberdeki kişiye iletişim bilgisi ekleme
Rehberdeki kişiden iletişim bilgisi kaldırma
Rehberdeki kişilerin listelenmesi
Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi
Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor talebi
Sistemin oluşturduğu raporların listelenmesi
Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi



## Docker Containers

| Image   | Port  | Host   | 
|---|---|---|
|  phonebook/gateway |  5000 | localhost  |
|  phonebook/phonebook | 5010  | localhost  |
|  phonebook/report | 5020  | localhost  |
|  phonebook/datacapture | 5201  | localhost  |
|  rabbitmq | 15672  | localhost  |


## Yapının Özeti 

Mikroservisleri tek endpoint'ten yönetebilmek için Ocelot eklentisi ile beraber bir adet  `Gateway` oluşturuldu.  
Mikroservislerin birbirleri arasındaki iletişimin asenkron ve düzenli olabilmesi için  `RabbitMQ` kullanıldı.
Rehbere ait Ekleme/Silme vb. işlemlerinin yapıldığı ve kendisine ait veritabanı `MongoDB` olan Phonebook mikroservisi oluşturuldu.
Rapolama işlemlerinin yapıldığı ve kendisine ait veritabanı (`Postgre`) olan Report mikroservisi oluşturuldu.

Rehbere bir kayıt eklendiğinde,silindiğinde ya da düzeltildiğinde  `Phonebook` mikroservisinde o kayıt `MongoDB` veritabanına eklenir ve `RabbitMQ` kullanılarak `Phonebook` mikroservisinden `DataCapture` mikroservisine mesaj gönderilir.Gelen mesajlar kuyruk sistemine göre tek tek işlenerek Rapor veritabanına yazılır . 

**Rapor Senaryosu**
1. **DataCapture mikroservisi entity operasyonlarını yakalayarak Rapor Servisi için oluşturulan (Postgre) veritabanına kaydeder.** Böylelikle Rapor istenildiğinde Rapor mikroservisi kendisine ait olan database 'den verileri çekip rapor oluşturur. Diğer mikroservisle ya da diğer mikroservisin veritabanını erişmek durumunda kalmaz .
2. Rapor mikro servisinden bir rapor istenildiğinde , servis asenkron olarak Rehber servisine bir mesaj göndererek verileri ister , veriler hazırlanıp geldiğinde ilgili verilerden raporu oluşturup kendi veritabnına kaydederek işlemi tamamlar .


![](img/microservice-architecture.png)

## Tech Stack

- .Net 5
- RabbitMQ
- Serilog & Seq 
- Redis
- Event Source
- CQRS && DDD
- MediaTR
- ReactJS
- İdentity Server
- Docker
- Ocelot
- PostgreSQL
- Entity Framework Core
- Ant Design
- Portainer


