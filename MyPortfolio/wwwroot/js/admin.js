// Admin paneli ortak davranışları (_AdminLayout yükler).

// Silme onayı: <form data-confirm="Emin misin?"> gönderilmeden önce onay sorar.
// Eskiden her formda onsubmit="return confirm('...')" vardı. İki sorunu vardı:
//  1) CSP inline olay tetikleyicilerini (onsubmit, onclick...) engeller.
//  2) Metne kayıt adı gömülünce (örn. "Microsoft'un Araçları") kesme işareti JS string'ini
//     erken kapatıyordu: kod hata verip onay sorulmadan siliyor, özel bir isim JS çalıştırabiliyordu.
// data-confirm'deki metin HTML olarak kodlanır, dataset ile düz metin okunur; kod olarak çalışmaz.
document.addEventListener('submit', function (e) {
    var form = e.target;
    if (!(form instanceof HTMLFormElement) || !form.dataset.confirm) return;

    if (!window.confirm(form.dataset.confirm)) {
        e.preventDefault();
    }
});
