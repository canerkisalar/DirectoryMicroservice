# .NET Microservice Örnek Rehber Uygulaması

Mikroservis mimarisi ile oluşturulmuş rehber uygulaması.

## Başlarken

Docker uygulamasının kurulu ve konfigürasyonu yapıldığına eminseniz , aşağıdaki kodu çalıştırarak `Phonebook Applicaton` uygulamasını hazır hale getirebilirsiz. 

```powershell
docker-compose up --build
```

## Seneryo

Birden fazla mikroservis kullanarak basit bir rehber uygulaması geliştirilmesi.

- Rehberde kişi oluşturma
- Rehberde kişi kaldırma
- Rehberdeki kişiye iletişim bilgisi ekleme
- Rehberdeki kişiden iletişim bilgisi kaldırma
- Rehberdeki kişilerin listelenmesi
- Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi
- Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor talebi
- Sistemin oluşturduğu raporların listelenmesi
- Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi



## Docker Containers

| Image   | Port  | Host   | 
|---|---|---|
|  phonebook/gateway |  5000 | localhost  |
|  phonebook/phonebook | 5010  | localhost  |
|  phonebook/report | 5020  | localhost  |
|  phonebook/datacapture | 5201  | localhost  |
|  rabbitmq | 15672  | localhost  |


## Yapının Özeti 


Yapı 4 mikro servis , 2 veritabanı ve 1 mesaj dinleyiciden oluşmaktadır. 

1. İlk mikro servisimiz **Gateway** 'dir . Yük dengelenmesi, kimlik doğrulama ya da yetkilendirme , diğer bütün mikroservisleri ortak bir yerden yönetimi için kullanılabilir. 

2.  Kişilerin rehbere eklenmesi , silinmesi , güncellenmesi vb. işlemleri yapan mikroservisimiz **Phonebook** dir.

3.  Raporların listelenmesi , raporların detay bilgilerinin getirilmesi , içerideki veriler doğrultusunda raporların hazırlamakla yükümlü olan   mikroservisimiz **Report** dir.

4.  Raporlama servisi için **Phonebook** mikro servisinin gönderdiği mesajları dinleyip , ilgili kayıtları rapor servisinin veritabanına yazan servisimiz **DataCapture** dir.
    

Yapının genel şeması aşağıdaki gibidir.

![](img/schema.png)

## Teknoloji ve Eklentiler 

- .Net 5
- RabbitMQ
- MassTransit
- Event Source
- CQRS && DDD
- Docker
- Ocelot
- PostgreSQL
- MongoDB
- Entity Framework Core
- Entity Framework Core In-Memory DB
- Portainer
- Fluent Validation
- AutoMapper
- Fluent Assertions
- Moq

# Ekran Görünütüleri 

## Gateway
![](img/Capture1.png)

## RabbitMQ
![](img/Capture2.png)

## Postgres
![](img/Capture3.png)

## Covarage
![](img/Capture4.png)


