// wwwroot/js/darkmode.js
// Chạy ngay trên <head> để tránh FOUC (chớp trắng màn hình)
(function() {
    const storedTheme = localStorage.getItem('theme');
    const systemTheme = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    const theme = storedTheme || systemTheme;
    document.documentElement.setAttribute('data-bs-theme', theme);
})();

document.addEventListener('DOMContentLoaded', () => {
    const toggleBtn = document.getElementById('theme-toggle');
    const icon = document.getElementById('theme-icon');

    const updateIcon = (theme) => {
        if (theme === 'dark') {
            icon.classList.remove('fa-moon', 'text-dark');
            icon.classList.add('fa-sun', 'text-warning');
            toggleBtn.title = "Bật chế độ Sáng";
        } else {
            icon.classList.remove('fa-sun', 'text-warning');
            icon.classList.add('fa-moon', 'text-dark');
            toggleBtn.title = "Bật chế độ Tối";
        }
    };

    if(toggleBtn && icon) {
        const currentTheme = document.documentElement.getAttribute('data-bs-theme');
        updateIcon(currentTheme);

        toggleBtn.addEventListener('click', () => {
            const current = document.documentElement.getAttribute('data-bs-theme');
            const newTheme = current === 'dark' ? 'light' : 'dark';
            
            // Gắn class theme-transition để chuyển mượt chỉ khi click
            document.documentElement.classList.add('theme-transition');
            
            document.documentElement.setAttribute('data-bs-theme', newTheme);
            localStorage.setItem('theme', newTheme);
            updateIcon(newTheme);
            
            // Gỡ class transition sau khi hiệu ứng kết thúc (300ms) để khi resize ko bị lag màu
            setTimeout(() => {
                document.documentElement.classList.remove('theme-transition');
            }, 300);
        });
    }
});
