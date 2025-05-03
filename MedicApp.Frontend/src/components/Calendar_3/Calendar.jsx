import { useState, useEffect } from "react";
import {
  Box,
  Typography,
  IconButton,
  Grid,
  Button,
  Tabs,
  Tab,
  TextField,
  Paper,
  Divider,
  CircularProgress,
  Alert,
  Snackbar,
} from "@mui/material";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
import EventBusyIcon from "@mui/icons-material/EventBusy";
import { BASE_API } from "../../constants/BASE_API";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
const daysOfWeek = ["Нд", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"];
const months = [
  "Січень",
  "Лютий",
  "Березень",
  "Квітень",
  "Травень",
  "Червень",
  "Липень",
  "Серпень",
  "Вересень",
  "Жовтень",
  "Листопад",
  "Грудень",
];

const Calendar = ({ onTimeSelect }) => {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [selectedDate, setSelectedDate] = useState(null);
  const [tabValue, setTabValue] = useState(0);
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);

  const [availabilityData, setAvailabilityData] = useState({});

  const formatDateString = (year, month, day) => {
    return `${year}-${String(month + 1).padStart(2, "0")}-${String(
      day
    ).padStart(2, "0")}`;
  };

  // Отримання даних з API
  const fetchScheduleData = async () => {
    setLoading(true);
    setError(null);

    try {
      const token = localStorage.getItem("accessToken");
      const response = await fetch(`${BASE_API}/schedule/intervas`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        throw new Error("Помилка отримання розкладу");
      }

      const data = await response.json();

      console.log(data);
      processScheduleData(data);
    } catch (err) {
      console.error("Помилка при отриманні даних розкладу:", err);
      setError(
        "Не вдалося завантажити розклад. Будь ласка, спробуйте пізніше."
      );
    } finally {
      setLoading(false);
    }
  };

  // Обробка даних розкладу у формат для календаря
  const processScheduleData = (scheduleData) => {
    const formattedData = {};

    scheduleData.forEach((schedule) => {
      const { date, intervals } = schedule;

      // Ініціалізація масиву для конкретної дати
      formattedData[date] = intervals.map((interval) => ({
        id: interval.id,
        start: interval.startTime.substring(0, 5), // Обрізаємо до формату HH:MM
        end: interval.endTime.substring(0, 5), // Обрізаємо до формату HH:MM
        isBooked: interval.isBooked,
        isComplete: interval.isComplete,
      }));
    });

    setAvailabilityData(formattedData);
  };

  // Завантаження даних при першому відкритті та зміні місяця
  useEffect(() => {
    fetchScheduleData();
  }, [currentDate.getMonth(), currentDate.getFullYear()]);

  // Обробник для попереднього місяця
  const handlePrevMonth = () => {
    setCurrentDate((prev) => {
      const newDate = new Date(prev);
      newDate.setMonth(newDate.getMonth() - 1);
      return newDate;
    });
  };

  // Обробник для наступного місяця
  const handleNextMonth = () => {
    setCurrentDate((prev) => {
      const newDate = new Date(prev);
      newDate.setMonth(newDate.getMonth() + 1);
      return newDate;
    });
  };

  // Обробник вибору дати
  const handleDateSelect = (date) => {
    setSelectedDate(date);
    setTabValue(0); // Перемикання на вкладку перегляду при виборі дати
  };

  // Обробник зміни вкладки
  const handleTabChange = (event, newValue) => {
    setTabValue(newValue);
  };

  // Збереження нового інтервалу
  const handleSaveInterval = async () => {
    if (!selectedDate || !startTime || !endTime) {
      setError("Виберіть дату та час!");
      return;
    }

    if (startTime >= endTime) {
      setError("Час початку повинен бути раніше часу закінчення!");
      return;
    }

    const year = selectedDate.getFullYear();
    const month = selectedDate.getMonth();
    const day = selectedDate.getDate();
    const dateString = formatDateString(year, month, day);
    const localDateStartTime = `${dateString}T${startTime}:00`;
    const localDateEndTime = `${dateString}T${endTime}:00`;

    setLoading(true);
    setError(null);

    try {
      const token = localStorage.getItem("accessToken");
      const response = await fetch(`${BASE_API}/doctor/schedule/interval`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          startTime: localDateStartTime,
          endTime: localDateEndTime,
        }),
      });

      if (!response.ok) {
        throw new Error("Помилка при збереженні інтервалу!");
      }

      const result = await response.json();
      console.log("Інтервал збережено на сервері:", result);

      // Оновлення даних після успішного додавання
      await fetchScheduleData();

      // Скидання форми
      setStartTime("");
      setEndTime("");
      setTabValue(0); // Повернення до вкладки перегляду
      setSuccess("Інтервал успішно додано");
    } catch (error) {
      console.error("❌ POST запит не вдався:", error);
      setError("Не вдалося зберегти інтервал. Спробуйте ще раз.");
    } finally {
      setLoading(false);
    }
  };

  // Видалення інтервалу
  const handleDeleteInterval = async (intervalId) => {
    setLoading(true);
    setError(null);

    try {
      const token = localStorage.getItem("accessToken");
      const response = await fetch(
        `${BASE_API}/doctor/schedule/interval/${intervalId}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (!response.ok) {
        throw new Error("Помилка при видаленні інтервалу!");
      }

      // Оновлення даних після успішного видалення
      await fetchScheduleData();
      setSuccess("Інтервал успішно видалено");
    } catch (error) {
      console.error("❌ DELETE запит не вдався:", error);
      setError("Не вдалося видалити інтервал. Спробуйте ще раз.");
    } finally {
      setLoading(false);
    }
  };

  // Створення сітки календаря
  const renderCalendarGrid = () => {
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();

    // Перший день місяця
    const firstDay = new Date(year, month, 1);
    const startDayOfWeek = firstDay.getDay();

    // Останній день місяця
    const lastDay = new Date(year, month + 1, 0);
    const totalDays = lastDay.getDate();

    // Масив днів календаря
    const calendarDays = [];

    // Додавання заголовків днів тижня
    daysOfWeek.forEach((day) => {
      calendarDays.push(
        <Grid item xs={12 / 7} key={`header-${day}`}>
          <Typography
            align="center"
            color="textSecondary"
            sx={{
              fontWeight: 500,
              fontSize: "0.875rem",
              padding: "10px 0",
            }}
          >
            {day}
          </Typography>
        </Grid>
      );
    });

    // Додавання порожніх клітинок перед першим днем
    for (let i = 0; i < startDayOfWeek; i++) {
      calendarDays.push(
        <Grid item xs={12 / 7} key={`empty-${i}`}>
          <Box sx={{ aspectRatio: "1/1" }}></Box>
        </Grid>
      );
    }

    // Додавання днів місяця
    for (let day = 1; day <= totalDays; day++) {
      const date = new Date(year, month, day);
      const dateString = formatDateString(year, month, day);
      const hasSlots = !!availabilityData[dateString];
      const hasBookedSlots =
        hasSlots && availabilityData[dateString].some((slot) => slot.isBooked);

      // Перевірка, чи вибрано цю дату
      const isSelected =
        selectedDate &&
        selectedDate.getDate() === day &&
        selectedDate.getMonth() === month &&
        selectedDate.getFullYear() === year;

      calendarDays.push(
        <Grid item xs={12 / 7} key={`day-${day}`}>
          <Box
            onClick={() => handleDateSelect(date)}
            sx={{
              aspectRatio: "1/1",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              borderRadius: "50%",
              cursor: "pointer",
              position: "relative",
              bgcolor: isSelected ? "primary.main" : "transparent",
              color: isSelected ? "primary.contrastText" : "inherit",
              "&:hover": {
                bgcolor: isSelected ? "primary.main" : "action.hover",
              },
            }}
          >
            {day}
            {hasSlots && (
              <Box
                sx={{
                  position: "absolute",
                  bottom: "4px",
                  left: "50%",
                  transform: "translateX(-50%)",
                  width: "4px",
                  height: "4px",
                  bgcolor: isSelected
                    ? "primary.contrastText"
                    : hasBookedSlots
                    ? "error.main"
                    : "primary.main",
                  borderRadius: "50%",
                }}
              />
            )}
          </Box>
        </Grid>
      );
    }

    return calendarDays;
  };

  // Компонент для відображення часових інтервалів
  const TimeSlots = () => {
    if (!selectedDate) {
      return (
        <Typography color="textSecondary" sx={{ fontStyle: "italic", py: 2 }}>
          Виберіть дату в календарі
        </Typography>
      );
    }

    const day = selectedDate.getDate();
    const month = selectedDate.getMonth();
    const year = selectedDate.getFullYear();
    const dateString = formatDateString(year, month, day);

    const timeSlots = availabilityData[dateString] || [];

    if (timeSlots.length === 0) {
      return (
        <Typography color="textSecondary" sx={{ fontStyle: "italic", py: 2 }}>
          Для цієї дати немає доступних годин. Натисніть "Додати" щоб створити
          новий інтервал.
        </Typography>
      );
    }

    return (
      <Box sx={{ mt: 2 }}>
        {timeSlots.map((slot) => (
          <Paper
            key={slot.id}
            elevation={1}
            sx={{
              p: 2,
              mb: 1,
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              cursor: slot.isBooked ? "default" : "pointer",
              bgcolor: slot.isBooked ? "rgba(211, 47, 47, 0.05)" : "inherit",
              borderLeft: slot.isBooked ? "4px solid #d32f2f" : "none",
              "&:hover": {
                boxShadow: slot.isBooked ? 1 : 3,
                transform: slot.isBooked ? "none" : "translateY(-2px)",
              },
              transition: "all 0.2s ease",
            }}
            onClick={() => {
              if (onTimeSelect) {
                onTimeSelect(slot.id);
              }
            }}
          >
            {console.log("Slot values:", {
              slot: slot,
              isComplete: slot.isComplete,
              isBooked: slot.isBooked,
            })}

            <Box sx={{ display: "flex", alignItems: "center" }}>
              {slot.isComplete && (
                <CheckCircleIcon
                  sx={{ color: "warning.main", mr: 1, fontSize: "1rem" }}
                />
              )}
              {slot.isBooked && !slot.isComplete && (
                <EventBusyIcon
                  sx={{ color: "error.main", mr: 1, fontSize: "1rem" }}
                />
              )}
              <Typography
                sx={{
                  color: slot.isComplete
                    ? "warning.main"
                    : slot.isBooked
                    ? "error.main"
                    : "text.primary",
                  fontWeight: slot.isComplete ? 600 : slot.isBooked ? 400 : 500,
                }}
              >
                {slot.start} - {slot.end}
                {slot.isComplete && " (Завершено)"}
                {slot.isBooked && !slot.isComplete && " (Зайнято)"}
              </Typography>
            </Box>

            <IconButton
              size="small"
              onClick={(e) => {
                e.stopPropagation();
                handleDeleteInterval(slot.id);
              }}
              disabled={slot.isBooked}
              sx={{
                opacity: slot.isBooked ? 0 : 0.3,
                "&:hover": {
                  opacity: slot.isBooked ? 0 : 1,
                  color: "error.main",
                },
              }}
            >
              <CloseIcon fontSize="small" />
            </IconButton>
          </Paper>
        ))}
      </Box>
    );
  };

  // Форма для додавання нових часових інтервалів
  const AddIntervalForm = () => {
    if (!selectedDate) {
      return (
        <Typography color="textSecondary" sx={{ fontStyle: "italic", py: 2 }}>
          Виберіть дату в календарі
        </Typography>
      );
    }

    return (
      <Box sx={{ mt: 2 }}>
        <Box sx={{ mb: 3 }}>
          <Typography
            variant="body2"
            sx={{ mb: 1, fontWeight: 500, color: "text.secondary" }}
          >
            Час початку:
          </Typography>
          <TextField
            type="time"
            fullWidth
            value={startTime}
            onChange={(e) => setStartTime(e.target.value)}
            size="small"
            disabled={loading}
          />
        </Box>

        <Box sx={{ mb: 3 }}>
          <Typography
            variant="body2"
            sx={{ mb: 1, fontWeight: 500, color: "text.secondary" }}
          >
            Час закінчення:
          </Typography>
          <TextField
            type="time"
            fullWidth
            value={endTime}
            onChange={(e) => setEndTime(e.target.value)}
            size="small"
            disabled={loading}
          />
        </Box>

        <Box sx={{ display: "flex", justifyContent: "space-between", mt: 4 }}>
          <Button
            variant="outlined"
            onClick={() => setTabValue(0)}
            disabled={loading}
          >
            Скасувати
          </Button>
          <Button
            variant="contained"
            onClick={handleSaveInterval}
            disabled={loading}
            startIcon={
              loading ? <CircularProgress size={16} color="inherit" /> : null
            }
          >
            {loading ? "Збереження..." : "Зберегти"}
          </Button>
        </Box>
      </Box>
    );
  };

  // Форматування дати для відображення
  const getFormattedSelectedDate = () => {
    if (!selectedDate) return null;

    const day = selectedDate.getDate();
    const month = selectedDate.getMonth();
    const year = selectedDate.getFullYear();

    return `${day} ${months[month]} ${year}`;
  };

  // Закриття повідомлень
  const handleCloseSnackbar = () => {
    setError(null);
    setSuccess(null);
  };

  return (
    <Box
      sx={{
        width: "100%",
        height: "100%",
        display: "flex",
        flexDirection: "column",
      }}
    >
      {/* Секція календаря */}
      <Box sx={{ p: 2, flex: 1 }}>
        {/* Заголовок календаря */}
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            mb: 3,
          }}
        >
          <IconButton onClick={handlePrevMonth} disabled={loading}>
            <ChevronLeftIcon />
          </IconButton>

          <Typography variant="h6">
            {months[currentDate.getMonth()]} {currentDate.getFullYear()}
          </Typography>

          <IconButton onClick={handleNextMonth} disabled={loading}>
            <ChevronRightIcon />
          </IconButton>
        </Box>

        {/* Сітка календаря */}
        {loading && !Object.keys(availabilityData).length ? (
          <Box sx={{ display: "flex", justifyContent: "center", py: 4 }}>
            <CircularProgress />
          </Box>
        ) : (
          <Grid container spacing={1}>
            {renderCalendarGrid()}
          </Grid>
        )}
      </Box>

      {/* Розділювач */}
      <Divider />

      {/* Секція часових інтервалів */}
      <Box sx={{ p: 2, maxHeight: "40%", overflowY: "auto" }}>
        {/* Вибрана дата та дії */}
        {selectedDate && (
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              mb: 2,
            }}
          >
            <Typography
              variant="subtitle1"
              sx={{
                position: "relative",
                pb: 1,
                fontWeight: 500,
                "&::after": {
                  content: '""',
                  position: "absolute",
                  bottom: 0,
                  left: 0,
                  width: 40,
                  height: 3,
                  bgcolor: "primary.main",
                },
              }}
            >
              {getFormattedSelectedDate()}
            </Typography>

            <Button
              variant="outlined"
              size="small"
              startIcon={<AddIcon />}
              onClick={() => setTabValue(1)}
              sx={{ borderRadius: 20 }}
              disabled={loading}
            >
              Додати
            </Button>
          </Box>
        )}

        {/* Вкладки */}
        <Tabs value={tabValue} onChange={handleTabChange} sx={{ mb: 2 }}>
          <Tab label="Перегляд" disabled={loading} />
          <Tab label="Додати інтервал" disabled={loading} />
        </Tabs>

        {/* Панелі вкладок */}
        <Box hidden={tabValue !== 0}>
          {loading && selectedDate ? (
            <Box sx={{ display: "flex", justifyContent: "center", py: 2 }}>
              <CircularProgress size={24} />
            </Box>
          ) : (
            <TimeSlots />
          )}
        </Box>
        <Box hidden={tabValue !== 1}>
          <AddIntervalForm />
        </Box>
      </Box>

      {/* Повідомлення про помилки та успіх */}
      <Snackbar
        open={!!error}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
      >
        <Alert
          onClose={handleCloseSnackbar}
          severity="error"
          sx={{ width: "100%" }}
        >
          {error}
        </Alert>
      </Snackbar>

      <Snackbar
        open={!!success}
        autoHideDuration={3000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
      >
        <Alert
          onClose={handleCloseSnackbar}
          severity="success"
          sx={{ width: "100%" }}
        >
          {success}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default Calendar;
