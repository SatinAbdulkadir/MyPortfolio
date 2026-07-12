document.addEventListener("DOMContentLoaded", function () {

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
    // Sidebar linkleri "/#bolum" formatında: aynı sayfadaysak smooth scroll,
    // farklı sayfadaysak (örn. proje detayı) normal navigasyonla ana sayfaya döner
    // ==========================================================================
    document.querySelectorAll('a[href^="#"], .sidebar-menu a').forEach(function (anchor) {
        anchor.addEventListener('click', function (e) {
            var hash = this.hash;
            var samePage = this.pathname === window.location.pathname;

            if (hash && samePage) {
                var target = document.querySelector(hash);
                if (target) {
                    e.preventDefault();
                    target.scrollIntoView({ behavior: 'smooth' });
                }
            }

            // Mobil/tablette menü açıksa kapat (scroll da olsa navigasyon da olsa)
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

            // CSRF token: sunucu her POST'ta bu token'ı doğrular (AutoValidateAntiforgeryToken)
            const antiForgeryInput = contactForm.querySelector('input[name="__RequestVerificationToken"]');
            const antiForgeryToken = antiForgeryInput ? antiForgeryInput.value : '';

            try {
                const response = await fetch('/Home/SendMessage', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json',
                        'cf-turnstile-response': turnstileResponse, // Token'ı Backend'e Header üzerinden yolluyoruz
                        'RequestVerificationToken': antiForgeryToken // CSRF koruması
                    },
                    body: JSON.stringify(messageData)
                });

                // Rate limit: kısa sürede çok fazla deneme yapıldıysa sunucu 429 döner
                if (response.status === 429) {
                    alert("Kısa sürede çok fazla mesaj denemesi yaptınız. Lütfen bir dakika bekleyip tekrar deneyin.");
                    return;
                }

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

    // ==========================================================================
    // 6. SCROLL REVEAL — İçerik görünüme girince yumuşakça belirir
    // ==========================================================================
    var prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    if (!prefersReducedMotion && 'IntersectionObserver' in window) {
        var revealSelectors = [
            '.section-title',
            '.about-info-content',
            '.timeline-item',
            '.skill-card',
            '#portfolio .row > [class*="col-"]',
            '.testimonial-item',
            '.contact-card',
            '.contact-header-info',
            '.message-form-wrapper',
            '.detail-section',
            '.detail-panel',
            '.other-projects .row > [class*="col-"]'
        ];

        var revealTargets = document.querySelectorAll(revealSelectors.join(', '));

        // Aynı grid içindeki kartlara kademeli gecikme ver (stagger efekti).
        // Ebeveyn elementin kendisi Map anahtarı olur — DOM taraması gerekmez.
        var parentGroups = new Map();
        revealTargets.forEach(function (el) {
            var count = (parentGroups.get(el.parentElement) || 0) + 1;
            parentGroups.set(el.parentElement, count);
            el.style.setProperty('--reveal-delay', (Math.min(count - 1, 8) * 0.07) + 's');
            el.classList.add('reveal');
        });

        var revealObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('revealed');
                    revealObserver.unobserve(entry.target);
                }
            });
        }, { threshold: 0.12, rootMargin: '0px 0px -40px 0px' });

        revealTargets.forEach(function (el) {
            revealObserver.observe(el);
        });
    }

    // ==========================================================================
    // 7. SCROLLSPY — Görünen bölümün menü linkini vurgular
    // ==========================================================================
    var menuLinks = document.querySelectorAll('.sidebar-menu a');

    if (menuLinks.length > 0 && 'IntersectionObserver' in window) {
        var sectionMap = {};
        menuLinks.forEach(function (link) {
            // link.hash "/#about" içinden "#about" kısmını verir; bölüm bu sayfada yoksa atlanır
            var section = link.hash ? document.querySelector(link.hash) : null;
            if (section) {
                sectionMap[section.id] = link;
            }
        });

        var spyObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting && sectionMap[entry.target.id]) {
                    menuLinks.forEach(function (l) { l.classList.remove('active'); });
                    sectionMap[entry.target.id].classList.add('active');
                }
            });
        }, { rootMargin: '-35% 0px -55% 0px' });

        Object.keys(sectionMap).forEach(function (id) {
            spyObserver.observe(document.getElementById(id));
        });
    }

    // ==========================================================================
    // 8. YUKARI DÖN BUTONU
    // ==========================================================================
    var backToTopBtn = document.getElementById('back-to-top');

    if (backToTopBtn) {
        window.addEventListener('scroll', function () {
            backToTopBtn.classList.toggle('visible', window.scrollY > 500);
        }, { passive: true });

        backToTopBtn.addEventListener('click', function () {
            window.scrollTo({ top: 0, behavior: prefersReducedMotion ? 'auto' : 'smooth' });
        });
    }

});