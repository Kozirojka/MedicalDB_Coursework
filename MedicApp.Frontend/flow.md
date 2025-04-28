```JavaScript
<!DOCTYPE html>
<html lang="uk">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Сучасний інтерактивний календар з додаванням інтервалів</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f9f9f9;
            color: #333;
            display: flex;
            min-height: 100vh;
        }
        
        .main-container {
            display: flex;
            width: 100%;
            max-width: 1200px;
            margin: 0 auto;
            box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
            background-color: white;
            position: relative;
            overflow: hidden;
        }
        
        .calendar-container {
            flex: 1;
            padding: 40px;
            transition: all 0.3s ease;
        }
        
        .calendar-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 30px;
        }
        
        .calendar-header button {
            background-color: #fff;
            color: #333;
            border: 1px solid #ddd;
            border-radius: 50%;
            width: 40px;
            height: 40px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 18px;
            cursor: pointer;
            transition: all 0.2s ease;
        }
        
        .calendar-header button:hover {
            background-color: #f1f1f1;
            transform: scale(1.05);
        }
        
        .calendar-header h2 {
            margin: 0;
            font-weight: 400;
            font-size: 24px;
            text-align: center;
            flex: 1;
        }
        
        .calendar-grid {
            display: grid;
            grid-template-columns: repeat(7, 1fr);
            gap: 10px;
        }
        
        .calendar-day-header {
            text-align: center;
            font-weight: 500;
            padding: 10px 0;
            font-size: 14px;
            color: #666;
        }
        
        .calendar-day {
            aspect-ratio: 1/1;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 50%;
            cursor: pointer;
            transition: all 0.2s ease;
            font-size: 15px;
            position: relative;
        }
        
        .calendar-day:hover {
            background-color: #f0f0f0;
        }
        
        .calendar-day.selected {
            background-color: #333;
            color: white;
        }
        
        .calendar-day.empty {
            cursor: default;
        }
        
        .calendar-day.has-slots::after {
            content: '';
            position: absolute;
            bottom: 4px;
            left: 50%;
            transform: translateX(-50%);
            width: 4px;
            height: 4px;
            background-color: #333;
            border-radius: 50%;
        }
        
        .calendar-day.selected.has-slots::after {
            background-color: white;
        }
        
        .sidebar {
            width: 0;
            background-color: #f1f1f1;
            transition: all 0.3s ease;
            overflow: hidden;
            box-shadow: -5px 0 15px rgba(0, 0, 0, 0.05);
        }
        
        .sidebar.active {
            width: 300px;
            padding: 40px 30px;
        }
        
        .selected-date {
            font-size: 20px;
            font-weight: 500;
            margin-bottom: 25px;
            color: #333;
            position: relative;
            padding-bottom: 15px;
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
        }
        
        .selected-date::after {
            content: '';
            position: absolute;
            bottom: 0;
            left: 0;
            width: 40px;
            height: 3px;
            background-color: #333;
        }
        
        .action-button {
            background-color: #333;
            color: white;
            border: none;
            border-radius: 20px;
            padding: 6px 12px;
            font-size: 12px;
            cursor: pointer;
            transition: all 0.2s ease;
            display: flex;
            align-items: center;
            gap: 5px;
        }
        
        .action-button:hover {
            background-color: #555;
        }
        
        .action-button.secondary {
            background-color: white;
            color: #333;
            border: 1px solid #ddd;
        }
        
        .action-button.secondary:hover {
            background-color: #f5f5f5;
        }
        
        .time-slots-container {
            margin-top: 20px;
        }
        
        .time-slot {
            padding: 15px;
            margin-bottom: 10px;
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
            transition: all 0.2s ease;
            cursor: pointer;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        
        .time-slot:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
        }
        
        .time-slot .delete-btn {
            opacity: 0;
            transition: opacity 0.2s ease;
            color: #999;
            border: none;
            background: none;
            cursor: pointer;
            font-size: 16px;
        }
        
        .time-slot:hover .delete-btn {
            opacity: 1;
        }
        
        .time-slot .delete-btn:hover {
            color: #ff4444;
        }
        
        .no-selection {
            color: #888;
            font-style: italic;
            padding: 20px 0;
        }
        
        /* Форма додавання інтервалу */
        .add-interval-form {
            margin-top: 20px;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-group label {
            display: block;
            margin-bottom: 8px;
            font-weight: 500;
            color: #555;
        }
        
        .form-group input {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 6px;
            font-size: 14px;
            box-sizing: border-box;
        }
        
        .form-actions {
            display: flex;
            justify-content: space-between;
            margin-top: 30px;
        }
        
        .btn {
            padding: 10px 20px;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.2s ease;
            border: none;
        }
        
        .btn-primary {
            background-color: #333;
            color: white;
        }
        
        .btn-primary:hover {
            background-color: #555;
        }
        
        .btn-secondary {
            background-color: #f1f1f1;
            color: #333;
        }
        
        .btn-secondary:hover {
            background-color: #e5e5e5;
        }
        
        .sidebar-view {
            display: none;
        }
        
        .sidebar-view.active {
            display: block;
        }
        
        .sidebar-nav {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
        }
        
        .sidebar-nav-btn {
            background: none;
            border: none;
            padding: 5px 10px;
            font-size: 14px;
            color: #777;
            cursor: pointer;
            position: relative;
        }
        
        .sidebar-nav-btn.active {
            color: #333;
            font-weight: 500;
        }
        
        .sidebar-nav-btn.active::after {
            content: '';
            position: absolute;
            bottom: -5px;
            left: 0;
            width: 100%;
            height: 2px;
            background-color: #333;
        }
        
        @media (max-width: 768px) {
            .main-container {
                flex-direction: column;
            }
            
            .sidebar.active {
                width: 100%;
                padding: 30px 20px;
            }
        }
    </style>
</head>
<body>
    <div class="main-container">
        <div class="calendar-container" id="calendar-container">
            <div class="calendar-header">
                <button id="prev-month">&#8249;</button>
                <h2 id="current-month">Квітень 2025</h2>
                <button id="next-month">&#8250;</button>
            </div>
            <div class="calendar-grid" id="calendar-grid">
                <!-- Calendar days will be generated by JavaScript -->
            </div>
        </div>
        
        <div class="sidebar" id="sidebar">
            <div class="sidebar-nav">
                <button id="view-tab" class="sidebar-nav-btn active">Перегляд</button>
                <button id="add-tab" class="sidebar-nav-btn">Додати інтервал</button>
            </div>
            
            <!-- Sidebar для перегляду інтервалів -->
            <div id="view-sidebar" class="sidebar-view active">
                <div class="selected-date" id="selected-date">
                    <span class="no-selection">Виберіть дату в календарі</span>
                </div>
                <div class="time-slots-container" id="time-slots">
                    <!-- Available time slots will be shown here -->
                </div>
            </div>
            
            <!-- Sidebar для додавання нового інтервалу -->
            <div id="add-sidebar" class="sidebar-view">
                <div class="selected-date">
                    <span id="add-selected-date">Виберіть дату в календарі</span>
                </div>
                <div class="add-interval-form">
                    <div class="form-group">
                        <label for="start-time">Час початку:</label>
                        <input type="time" id="start-time" required>
                    </div>
                    <div class="form-group">
                        <label for="end-time">Час закінчення:</label>
                        <input type="time" id="end-time" required>
                    </div>
                    <div class="form-actions">
                        <button type="button" class="btn btn-secondary" id="cancel-btn">Скасувати</button>
                        <button type="button" class="btn btn-primary" id="save-btn">Зберегти</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        // Мок-дані в форматі JSON для демонстрації
        let availabilityData = {
            "2025-04-28": [
                { "start": "09:00", "end": "10:30" },
                { "start": "11:00", "end": "12:30" },
                { "start": "14:00", "end": "15:30" }
            ],
            "2025-04-29": [
                { "start": "10:00", "end": "11:30" },
                { "start": "13:00", "end": "14:30" }
            ],
            "2025-04-30": [
                { "start": "09:30", "end": "11:00" },
                { "start": "12:00", "end": "13:30" },
                { "start": "15:00", "end": "16:30" }
            ],
            "2025-05-01": [
                { "start": "09:00", "end": "10:00" },
                { "start": "11:00", "end": "12:00" },
                { "start": "13:00", "end": "14:00" },
                { "start": "15:00", "end": "16:00" }
            ],
            "2025-05-02": [
                { "start": "10:00", "end": "12:00" },
                { "start": "14:00", "end": "16:00" }
            ],
            "2025-05-05": [
                { "start": "09:00", "end": "10:30" },
                { "start": "11:30", "end": "13:00" }
            ],
            "2025-05-06": [
                { "start": "14:00", "end": "16:30" }
            ]
        };

        let currentDate = new Date();
        let selectedDate = null;
        
        // Дні тижня українською
        const daysOfWeek = ['Нд', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'];
        
        // Місяці українською
        const months = [
            'Січень', 'Лютий', 'Березень', 'Квітень', 'Травень', 'Червень',
            'Липень', 'Серпень', 'Вересень', 'Жовтень', 'Листопад', 'Грудень'
        ];
        
        // Ініціалізація календаря
        function initCalendar() {
            document.getElementById('prev-month').addEventListener('click', () => {
                currentDate.setMonth(currentDate.getMonth() - 1);
                renderCalendar();
            });
            
            document.getElementById('next-month').addEventListener('click', () => {
                currentDate.setMonth(currentDate.getMonth() + 1);
                renderCalendar();
            });
            
            // Перемикання між вкладками перегляду та додавання
            document.getElementById('view-tab').addEventListener('click', () => {
                switchTab('view');
            });
            
            document.getElementById('add-tab').addEventListener('click', () => {
                switchTab('add');
            });
            
            // Кнопки форми додавання інтервалу
            document.getElementById('cancel-btn').addEventListener('click', () => {
                switchTab('view');
            });
            
            document.getElementById('save-btn').addEventListener('click', () => {
                saveNewInterval();
            });
            
            renderCalendar();
        }
        
        // Перемикання між вкладками sidebar
        function switchTab(tab) {
            // Активація кнопки вкладки
            document.getElementById('view-tab').classList.toggle('active', tab === 'view');
            document.getElementById('add-tab').classList.toggle('active', tab === 'add');
            
            // Показ відповідного вмісту
            document.getElementById('view-sidebar').classList.toggle('active', tab === 'view');
            document.getElementById('add-sidebar').classList.toggle('active', tab === 'add');
            
            // Якщо перейшли на вкладку додавання, оновити заголовок з датою
            if (tab === 'add' && selectedDate) {
                const day = selectedDate.getDate();
                const month = selectedDate.getMonth();
                const year = selectedDate.getFullYear();
                document.getElementById('add-selected-date').textContent = `${day} ${months[month]} ${year}`;
            }
        }
        
        // Збереження нового інтервалу
        function saveNewInterval() {
            if (!selectedDate) {
                alert('Спочатку виберіть дату!');
                return;
            }
            
            const startTime = document.getElementById('start-time').value;
            const endTime = document.getElementById('end-time').value;
            
            if (!startTime || !endTime) {
                alert('Введіть час початку та закінчення!');
                return;
            }
            
            if (startTime >= endTime) {
                alert('Час початку повинен бути раніше часу закінчення!');
                return;
            }
            
            // Форматування дати для пошуку в JSON
            const year = selectedDate.getFullYear();
            const month = selectedDate.getMonth();
            const day = selectedDate.getDate();
            const dateString = formatDateString(year, month, day);
            
            // Додавання нового інтервалу
            if (!availabilityData[dateString]) {
                availabilityData[dateString] = [];
            }
            
            availabilityData[dateString].push({
                start: startTime,
                end: endTime
            });
            
            // Сортування інтервалів за часом
            availabilityData[dateString].sort((a, b) => a.start.localeCompare(b.start));
            
            // Очищення форми
            document.getElementById('start-time').value = '';
            document.getElementById('end-time').value = '';
            
            // Повернення на вкладку перегляду
            switchTab('view');
            
            // Оновлення відображення
            updateSidebar();
            
            // Додавання маркера до календаря, якщо його ще не було
            const selectedDayElement = document.querySelector('.calendar-day.selected');
            if (selectedDayElement && !selectedDayElement.classList.contains('has-slots')) {
                selectedDayElement.classList.add('has-slots');
            }
        }
        
        // Видалення інтервалу
        function deleteInterval(dateString, index) {
            // Видалення інтервалу з даних
            availabilityData[dateString].splice(index, 1);
            
            // Якщо не залишилось інтервалів для цієї дати, видаляємо запис
            if (availabilityData[dateString].length === 0) {
                delete availabilityData[dateString];
                
                // Видалення маркера з календаря
                const selectedDayElement = document.querySelector('.calendar-day.selected');
                if (selectedDayElement) {
                    selectedDayElement.classList.remove('has-slots');
                }
            }
            
            // Оновлення відображення
            updateSidebar();
        }
        
        // Генерація календаря
        function renderCalendar() {
            const year = currentDate.getFullYear();
            const month = currentDate.getMonth();
            
            // Оновлення заголовка
            document.getElementById('current-month').textContent = `${months[month]} ${year}`;
            
            const calendarGrid = document.getElementById('calendar-grid');
            calendarGrid.innerHTML = '';
            
            // Додавання назв днів тижня
            for (let i = 0; i < 7; i++) {
                const dayHeader = document.createElement('div');
                dayHeader.className = 'calendar-day-header';
                dayHeader.textContent = daysOfWeek[i];
                calendarGrid.appendChild(dayHeader);
            }
            
            // Перший день місяця
            const firstDay = new Date(year, month, 1);
            const startDayOfWeek = firstDay.getDay();
            
            // Останній день місяця
            const lastDay = new Date(year, month + 1, 0);
            const totalDays = lastDay.getDate();
            
            // Додавання порожніх клітинок перед першим днем місяця
            for (let i = 0; i < startDayOfWeek; i++) {
                const emptyDay = document.createElement('div');
                emptyDay.className = 'calendar-day empty';
                calendarGrid.appendChild(emptyDay);
            }
            
            // Додавання днів місяця
            for (let day = 1; day <= totalDays; day++) {
                const dayElement = document.createElement('div');
                dayElement.className = 'calendar-day';
                dayElement.textContent = day;
                
                const dateString = formatDateString(year, month, day);
                
                // Якщо цей день має доступні години
                if (availabilityData[dateString]) {
                    dayElement.classList.add('has-slots');
                }
                
                // Додавання обробника подій для вибору дня
                dayElement.addEventListener('click', () => {
                    // Зняття виділення з попереднього вибраного дня
                    const selected = document.querySelector('.calendar-day.selected');
                    if (selected) {
                        selected.classList.remove('selected');
                    }
                    
                    // Виділення обраного дня
                    dayElement.classList.add('selected');
                    
                    // Оновлення інформації про вибраний день
                    selectedDate = new Date(year, month, day);
                    
                    // Показ бічної панелі
                    document.getElementById('sidebar').classList.add('active');
                    switchTab('view');
                    
                    updateSidebar();
                });
                
                calendarGrid.appendChild(dayElement);
            }
        }
        
        // Форматування дати в рядок 'YYYY-MM-DD'
        function formatDateString(year, month, day) {
            return `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
        }
        
        // Оновлення бічної панелі з доступними годинами
        function updateSidebar() {
            const selectedDateElement = document.getElementById('selected-date');
            const timeSlotsElement = document.getElementById('time-slots');
            
            if (!selectedDate) {
                selectedDateElement.innerHTML = '<span class="no-selection">Виберіть дату в календарі</span>';
                timeSlotsElement.innerHTML = '';
                return;
            }
            
            const day = selectedDate.getDate();
            const month = selectedDate.getMonth();
            const year = selectedDate.getFullYear();
            
            // Форматування вибраної дати для показу
            const formattedDate = `${day} ${months[month]} ${year}`;
            
            // Додамо кнопку для переходу на вкладку додавання
            selectedDateElement.innerHTML = `
                <span>${formattedDate}</span>
                <button class="action-button" id="add-interval-btn">
                    <span>+</span> Додати
                </button>
            `;
            
            // Додаємо обробник для кнопки додавання
            document.getElementById('add-interval-btn').addEventListener('click', () => {
                switchTab('add');
            });
            
            // Форматування дати для пошуку в JSON
            const dateString = formatDateString(year, month, day);
            
            // Очищення попередніх слотів часу
            timeSlotsElement.innerHTML = '';
            
            // Перевірка наявності доступних годин для вибраної дати
            if (availabilityData[dateString]) {
                const timeSlots = availabilityData[dateString];
                
                // Створення елементів для кожного доступного проміжку часу
                timeSlots.forEach((slot, index) => {
                    const timeSlotElement = document.createElement('div');
                    timeSlotElement.className = 'time-slot';
                    timeSlotElement.innerHTML = `
                        <span>${slot.start} - ${slot.end}</span>
                        <button class="delete-btn">&times;</button>
                    `;
                    
                    // Додаємо обробник видалення
                    const deleteBtn = timeSlotElement.querySelector('.delete-btn');
                    deleteBtn.addEventListener('click', (e) => {
                        e.stopPropagation();
                        deleteInterval(dateString, index);
                    });
                    
                    timeSlotsElement.appendChild(timeSlotElement);
                });
            } else {
                // Якщо немає доступних годин
                const noTimeSlots = document.createElement('div');
                noTimeSlots.className = 'no-selection';
                noTimeSlots.textContent = 'Для цієї дати немає доступних годин. Натисніть "Додати" щоб створити новий інтервал.';
                timeSlotsElement.appendChild(noTimeSlots);
            }
        }
        
        // Запуск після завантаження сторінки
        document.addEventListener('DOMContentLoaded', initCalendar);
    </script>
</body>
</html>
```