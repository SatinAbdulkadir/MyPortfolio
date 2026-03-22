document.addEventListener("DOMContentLoaded", function () {

    // 1. Skill Barları İçin Kontrol ve Çalıştırma
    const skillSection = document.getElementById('skills');
    const progressBars = document.querySelectorAll('.progress-bar');

    if (skillSection && progressBars.length > 0) {
        const showProgress = () => {
            progressBars.forEach(progressBar => {
                const value = progressBar.dataset.width;
                progressBar.style.width = `${value}%`;
            });
        }
        window.addEventListener('scroll', () => {
            const sectionPos = skillSection.getBoundingClientRect().top;
            const screenPos = window.innerHeight / 1.2;
            if (sectionPos < screenPos) { showProgress(); }
        });
    }

    // 2. Swiper (Referanslar) Motoru
    // Eğer kütüphane yüklüyse ve element varsa çalıştır
    if (document.querySelector('.testimonials-slider')) {
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

    // 3. Yumuşak Kaydırma
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({ behavior: 'smooth' });
            }
        });
    });
});