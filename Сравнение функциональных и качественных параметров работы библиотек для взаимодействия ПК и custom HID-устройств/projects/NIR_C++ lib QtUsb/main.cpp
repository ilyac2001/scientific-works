#include <QCoreApplication>
#include <QObject>
#include <QHidDevice>
#include <QTimer>
#include <ctime>

#define VID 0x0483
#define PID 0x5750

#define SIZE_BUFFER_READ_DATA 2
#define SIZE_BUFFER_WRITE_DATA 2

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);
    QTimer timer;
    QObject::connect(&timer, SIGNAL(timeout()), &a, SLOT(quit()));
    timer.start(1000);
    qInfo("Run: C++, QtUsb");

    srand(time(0));
    QHidDevice host_usb;
    QByteArray buf(SIZE_BUFFER_WRITE_DATA, (char)0x00);

    qDebug("Opening");
    if (host_usb.open(VID, PID)) {
        qInfo("Device open!");
    } else {
        qWarning("Could not open device!");
    }

    buf[0] = 0x02;
    for(int i = 0; i < 1000; i++){
        qDebug() << "Writing: " << i << " : " << buf;
        if (host_usb.write(&(QByteArray)buf, buf.size()) < 0) {
            qWarning("Write failed!");
        }
    }

    buf[0] = 0x01;
    for(int i = 0 ; i < 1000; i++){
        host_usb.read(&buf, buf.size());
        qDebug() << "Reading: " << i << " : " << buf;
    }

    qDebug("Closing");
    if (host_usb.isOpen()) {
        host_usb.close();
    }

    qDebug() << "Runtime = " << clock()/1000.0;

    return a.exec();
}
