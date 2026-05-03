document.addEventListener("DOMContentLoaded", function () {

    // ==========================================================================
    // 1. SKILL BARLARI — Scroll tetiklemeli animasyon
    // ==========================================================================
    const skillSection = document.getElementById('skills');
    const progressBars = document.querySelectorAll('.progress-bar');

    if (skillSection && progressBars.length > 0) {
        const showProgress = () => {
            progressBars.forEach(function (progressBar) {
                const value = progressBar.dataset.width;
                progressBar.style.width = value + '%';
            });
        };

        window.addEventListener('scroll', function () {
            const sectionPos = skillSection.getBoundingClientRect().top;
            const screenPos = window.innerHeight / 1.2;
            if (sectionPos < screenPos) {
                showProgress();
            }
        });
    }

    // ==========================================================================
    // 2. SWIPER — Referanslar slider (tek init, çift çalışmayı önler)
    // ==========================================================================
    var swiperEl = document.querySelector('.testimonials-slider');
    if (swiperEl && typeof Swiper !== 'undefined') {
        new Swiper('.testimonials-slider', {
            speed: 600,
            loop: true,
            autoplay: {
                delay: 5000,
                disableOnInteraction: false
            },
            slidesPerView: 'auto',
            pagination: {
                el: '.swiper-pagination',
                type: 'bullets',
                clickable: true
            },
            breakpoints: {
                320: { slidesPerView: 1, spaceBetween: 20 },
                1200: { slidesPerView: 2, spaceBetween: 20 }
            }
        });
    }

    // ==========================================================================
    // 3. YUMUŞAK KAYDIRMA — Hamburger kapatma ile entegre
    // ==========================================================================
    document.querySelectorAll('a[href^="#"]').forEach(function (anchor) {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();

            var target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({ behavior: 'smooth' });
            }

            // Mobil/tablette menü açıksa smooth scroll sonrası kapat
            var sidebar = document.querySelector('.sidebar-wrapper');
            var overlay = document.getElementById('sidebar-overlay');
            var btn = document.getElementById('hamburger-btn');

            if (sidebar && btn && window.getComputedStyle(btn).display !== 'none') {
                sidebar.classList.remove('open');
                overlay && overlay.classList.remove('active');
                btn.classList.remove('active');
                btn.setAttribute('aria-expanded', 'false');
                document.body.style.overflow = '';
            }
        });
    });

    // ==========================================================================
    // 4. HAMBURGEr MENÜ — Mobil/Tablet Toggle
    // ==========================================================================
    var hamburgerBtn = document.getElementById('hamburger-btn');
    var sidebarEl = document.querySelector('.sidebar-wrapper');
    var overlayEl = document.getElementById('sidebar-overlay');

    if (hamburgerBtn && sidebarEl && overlayEl) {

        function openSidebar() {
            sidebarEl.classList.add('open');
            overlayEl.classList.add('active');
            hamburgerBtn.classList.add('active');
            hamburgerBtn.setAttribute('aria-expanded', 'true');
            document.body.style.overflow = 'hidden';
        }

        function closeSidebar() {
            sidebarEl.classList.remove('open');
            overlayEl.classList.remove('active');
            hamburgerBtn.classList.remove('active');
            hamburgerBtn.setAttribute('aria-expanded', 'false');
            document.body.style.overflow = '';
        }

        // Butona tıkla → aç/kapat
        hamburgerBtn.addEventListener('click', function () {
            sidebarEl.classList.contains('open') ? closeSidebar() : openSidebar();
        });

        // Overlay'e tıkla → kapat
        overlayEl.addEventListener('click', closeSidebar);

        // Ekran genişleyince (masaüstüne dönünce) temizle
        window.addEventListener('resize', function () {
            if (window.innerWidth > 1024) {
                closeSidebar();
            }
        });
    }

    // ==========================================================================
    // 5. İLETİŞİM FORMU (AJAX / Fetch API) - v1.0 Kurumsal Entegrasyon
    // ==========================================================================
    const contactForm = document.getElementById('contactForm');

    if (contactForm) {
        contactForm.addEventListener('submit', async function (e) {
            e.preventDefault(); // Sayfa yenilenmesi (Postback) engellendi

            const btnSubmit = document.getElementById('btnSubmit');
            const originalBtnText = btnSubmit.innerHTML;

            // 1. Fail-Fast: Cloudflare Turnstile Token Kontrolü
            const turnstileInput = document.querySelector('[name="cf-turnstile-response"]');
            const turnstileResponse = turnstileInput ? turnstileInput.value : '';

            if (!turnstileResponse) {
                alert("Lütfen güvenlik doğrulamasını (CAPTCHA) tamamlayın.");
                return; // Token yoksa hiç sunucuya gitme, işlemi anında kes
            }

            // Çoklu tıklama (Spam) engeli ve UX geri bildirimi
            btnSubmit.disabled = true;
            btnSubmit.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> İşleniyor...';

            // DTO ile birebir eşleşen JSON modeli
            const messageData = {
                NameSurname: document.getElementById('NameSurname').value.trim(),
                Email: document.getElementById('Email').value.trim(),
                Subject: document.getElementById('Subject').value.trim(),
                MessageDetail: document.getElementById('MessageDetail').value.trim()
            };

            try {
                const response = await fetch('/Home/SendMessage', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json',
                        'cf-turnstile-response': turnstileResponse // Token'ı Backend'e Header üzerinden yolluyoruz
                    },
                    body: JSON.stringify(messageData)
                });

                // Sunucu 500 hatası verirse json parse etmeye çalışıp patlamasın diye güvenlik kontrolü
                if (!response.ok && response.status !== 400 && response.status !== 500) {
                    throw new Error(`HTTP Error: ${response.status}`);
                }

                const result = await response.json();

                if (result.success) {
                    // İşlem tamamen başarılı
                    alert(result.message);
                    contactForm.reset();
                } else {
                    // KURUMSAL STANDART: Defansif Hata Yönetimi
                    let finalErrorMessage = result.message || "İşlem sırasında beklenmeyen bir hata oluştu.";

                    // Eğer validasyon hatası varsa ve dizi (array) formatında geldiyse mesaja ekle
                    if (result.errors && Array.isArray(result.errors) && result.errors.length > 0) {
                        finalErrorMessage += "\n\nDetaylar:\n" + result.errors.join('\n');
                    }

                    alert(finalErrorMessage);
                }
            } catch (error) {
                console.error('API Bağlantı Hatası:', error);
                alert('Sunucuyla iletişim kurulamadı. Lütfen internet bağlantınızı kontrol edip tekrar deneyin.');
            } finally {
                // Hata da olsa başarılı da olsa butonu aktif hale getir
                btnSubmit.disabled = false;
                btnSubmit.innerHTML = originalBtnText;

                // UX Kuralı: İşlem bittikten sonra Turnstile widget'ını sıfırla ki tekrar kullanılabilsin
                if (typeof turnstile !== 'undefined') {
                    turnstile.reset();
                }
            }
        });
    }

});