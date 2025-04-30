import { useState } from 'react';
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
  Divider
} from '@mui/material';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import AddIcon from '@mui/icons-material/Add';
import CloseIcon from '@mui/icons-material/Close';

// Українські назви днів тижня та місяців
const daysOfWeek = ['Нд', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'];
const months = [
  'Січень', 'Лютий', 'Березень', 'Квітень', 'Травень', 'Червень',
  'Липень', 'Серпень', 'Вересень', 'Жовтень', 'Листопад', 'Грудень'
];

const Calendar = ({ onTimeSelect, onClose }) => {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [selectedDate, setSelectedDate] = useState(null);
  const [tabValue, setTabValue] = useState(0);
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');
  
  // Mock data for available time slots - replace with actual API data
  const [availabilityData, setAvailabilityData] = useState({
    "2025-04-28": [
      { id: "101", start: "09:00", end: "10:30" },
      { id: "102", start: "11:00", end: "12:30" },
      { id: "103", start: "14:00", end: "15:30" }
    ],
    "2025-04-29": [
      { id: "104", start: "10:00", end: "11:30" },
      { id: "105", start: "13:00", end: "14:30" }
    ],
    "2025-04-30": [
      { id: "106", start: "09:30", end: "11:00" },
      { id: "107", start: "12:00", end: "13:30" },
      { id: "108", start: "15:00", end: "16:30" }
    ],
    "2025-05-01": [
      { id: "109", start: "09:00", end: "10:00" },
      { id: "110", start: "11:00", end: "12:00" },
      { id: "111", start: "13:00", end: "14:00" },
      { id: "112", start: "15:00", end: "16:00" }
    ],
    "2025-05-02": [
      { id: "113", start: "10:00", end: "12:00" },
      { id: "114", start: "14:00", end: "16:00" }
    ],
    "2025-05-05": [
      { id: "115", start: "09:00", end: "10:30" },
      { id: "116", start: "11:30", end: "13:00" }
    ],
    "2025-05-06": [
      { id: "117", start: "14:00", end: "16:30" }
    ]
  });

  // Format date as YYYY-MM-DD
  const formatDateString = (year, month, day) => {
    return `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
  };

  // Handle previous month button
  const handlePrevMonth = () => {
    setCurrentDate(prev => {
      const newDate = new Date(prev);
      newDate.setMonth(newDate.getMonth() - 1);
      return newDate;
    });
  };

  // Handle next month button
  const handleNextMonth = () => {
    setCurrentDate(prev => {
      const newDate = new Date(prev);
      newDate.setMonth(newDate.getMonth() + 1);
      return newDate;
    });
  };

  // Handle date selection
  const handleDateSelect = (date) => {
    setSelectedDate(date);
    setTabValue(0); // Switch to view tab when selecting date
  };

  // Handle tab change
  const handleTabChange = (event, newValue) => {
    setTabValue(newValue);
  };

  // Save new time interval
  const handleSaveInterval = () => {
    if (!selectedDate || !startTime || !endTime) {
      alert('Виберіть дату та час!');
      return;
    }

    if (startTime >= endTime) {
      alert('Час початку повинен бути раніше часу закінчення!');
      return;
    }

    const year = selectedDate.getFullYear();
    const month = selectedDate.getMonth();
    const day = selectedDate.getDate();
    const dateString = formatDateString(year, month, day);

    // Generate a mock ID - in a real app, this would come from the backend
    const newId = `${Date.now()}`;
    
    // Add new interval
    setAvailabilityData(prev => {
      const newData = { ...prev };
      if (!newData[dateString]) {
        newData[dateString] = [];
      }
      
      newData[dateString] = [
        ...newData[dateString],
        { id: newId, start: startTime, end: endTime }
      ].sort((a, b) => a.start.localeCompare(b.start));
      
      return newData;
    });

    // Reset form
    setStartTime('');
    setEndTime('');
    setTabValue(0); // Switch back to view tab
  };

  // Delete time interval
  const handleDeleteInterval = (dateString, index) => {
    setAvailabilityData(prev => {
      const newData = { ...prev };
      newData[dateString].splice(index, 1);
      
      if (newData[dateString].length === 0) {
        delete newData[dateString];
      }
      
      return newData;
    });
  };

  // Generate calendar grid
  const renderCalendarGrid = () => {
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();
    
    // First day of month
    const firstDay = new Date(year, month, 1);
    const startDayOfWeek = firstDay.getDay();
    
    // Last day of month
    const lastDay = new Date(year, month + 1, 0);
    const totalDays = lastDay.getDate();
    
    // Calendar days array
    const calendarDays = [];
    
    // Add day headers
    daysOfWeek.forEach(day => {
      calendarDays.push(
        <Grid item xs={12/7} key={`header-${day}`}>
          <Typography 
            align="center" 
            color="textSecondary" 
            sx={{ 
              fontWeight: 500, 
              fontSize: '0.875rem', 
              padding: '10px 0'
            }}
          >
            {day}
          </Typography>
        </Grid>
      );
    });
    
    // Add empty cells before first day
    for (let i = 0; i < startDayOfWeek; i++) {
      calendarDays.push(
        <Grid item xs={12/7} key={`empty-${i}`}>
          <Box sx={{ aspectRatio: '1/1' }}></Box>
        </Grid>
      );
    }
    
    // Add month days
    for (let day = 1; day <= totalDays; day++) {
      const date = new Date(year, month, day);
      const dateString = formatDateString(year, month, day);
      const hasSlots = !!availabilityData[dateString];
      
      // Check if this date is selected
      const isSelected = selectedDate && 
        selectedDate.getDate() === day && 
        selectedDate.getMonth() === month && 
        selectedDate.getFullYear() === year;
      
      calendarDays.push(
        <Grid item xs={12/7} key={`day-${day}`}>
          <Box
            onClick={() => handleDateSelect(date)}
            sx={{
              aspectRatio: '1/1',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              borderRadius: '50%',
              cursor: 'pointer',
              position: 'relative',
              bgcolor: isSelected ? 'primary.main' : 'transparent',
              color: isSelected ? 'primary.contrastText' : 'inherit',
              '&:hover': {
                bgcolor: isSelected ? 'primary.main' : 'action.hover'
              },
              '&::after': hasSlots ? {
                content: '""',
                position: 'absolute',
                bottom: '4px',
                left: '50%',
                transform: 'translateX(-50%)',
                width: '4px',
                height: '4px',
                bgcolor: isSelected ? 'primary.contrastText' : 'primary.main',
                borderRadius: '50%'
              } : {}
            }}
          >
            {day}
          </Box>
        </Grid>
      );
    }
    
    return calendarDays;
  };

  // Component for displaying time slots
  const TimeSlots = () => {
    if (!selectedDate) {
      return (
        <Typography color="textSecondary" sx={{ fontStyle: 'italic', py: 2 }}>
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
        <Typography color="textSecondary" sx={{ fontStyle: 'italic', py: 2 }}>
          Для цієї дати немає доступних годин. 
          Натисніть "Додати" щоб створити новий інтервал.
        </Typography>
      );
    }
    
    return (
      <Box sx={{ mt: 2 }}>
        {timeSlots.map((slot, index) => (
          <Paper
            key={slot.id || index}
            elevation={1}
            sx={{
              p: 2,
              mb: 1,
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              cursor: 'pointer',
              '&:hover': {
                boxShadow: 3,
                transform: 'translateY(-2px)',
              },
              transition: 'all 0.2s ease'
            }}
            onClick={() => onTimeSelect && onTimeSelect(slot.id)}
          >
            <Typography>{slot.start} - {slot.end}</Typography>
            <IconButton
              size="small"
              onClick={(e) => {
                e.stopPropagation();
                handleDeleteInterval(dateString, index);
              }}
              sx={{
                opacity: 0,
                '&:hover': {
                  opacity: 1,
                  color: 'error.main'
                },
                ':hover': {
                  opacity: 1
                }
              }}
            >
              <CloseIcon fontSize="small" />
            </IconButton>
          </Paper>
        ))}
      </Box>
    );
  };

  // Form for adding new time slots
  const AddIntervalForm = () => {
    if (!selectedDate) {
      return (
        <Typography color="textSecondary" sx={{ fontStyle: 'italic', py: 2 }}>
          Виберіть дату в календарі
        </Typography>
      );
    }

    return (
      <Box sx={{ mt: 2 }}>
        <Box sx={{ mb: 3 }}>
          <Typography variant="body2" sx={{ mb: 1, fontWeight: 500, color: 'text.secondary' }}>
            Час початку:
          </Typography>
          <TextField
            type="time"
            fullWidth
            value={startTime}
            onChange={(e) => setStartTime(e.target.value)}
            size="small"
          />
        </Box>
        
        <Box sx={{ mb: 3 }}>
          <Typography variant="body2" sx={{ mb: 1, fontWeight: 500, color: 'text.secondary' }}>
            Час закінчення:
          </Typography>
          <TextField
            type="time"
            fullWidth
            value={endTime}
            onChange={(e) => setEndTime(e.target.value)}
            size="small"
          />
        </Box>
        
        <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 4 }}>
          <Button 
            variant="outlined" 
            onClick={() => setTabValue(0)}
          >
            Скасувати
          </Button>
          <Button 
            variant="contained" 
            onClick={handleSaveInterval}
          >
            Зберегти
          </Button>
        </Box>
      </Box>
    );
  };

  // Format date for display
  const getFormattedSelectedDate = () => {
    if (!selectedDate) return null;
    
    const day = selectedDate.getDate();
    const month = selectedDate.getMonth();
    const year = selectedDate.getFullYear();
    
    return `${day} ${months[month]} ${year}`;
  };

  return (
    <Box sx={{ width: '100%', height: '100%', display: 'flex', flexDirection: 'column' }}>
      {/* Close button */}
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', p: 1 }}>
        <IconButton onClick={onClose}>
          <CloseIcon />
        </IconButton>
      </Box>
      
      {/* Calendar section */}
      <Box sx={{ p: 2, flex: 1 }}>
        {/* Calendar header */}
        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 3 }}>
          <IconButton onClick={handlePrevMonth}>
            <ChevronLeftIcon />
          </IconButton>
          
          <Typography variant="h6">
            {months[currentDate.getMonth()]} {currentDate.getFullYear()}
          </Typography>
          
          <IconButton onClick={handleNextMonth}>
            <ChevronRightIcon />
          </IconButton>
        </Box>
        
        {/* Calendar grid */}
        <Grid container spacing={1}>
          {renderCalendarGrid()}
        </Grid>
      </Box>
      
      {/* Divider */}
      <Divider />
      
      {/* Time slots section */}
      <Box sx={{ p: 2, maxHeight: '40%', overflowY: 'auto' }}>
        {/* Selected date and actions */}
        {selectedDate && (
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
            <Typography 
              variant="subtitle1" 
              sx={{ 
                position: 'relative',
                pb: 1,
                fontWeight: 500,
                '&::after': {
                  content: '""',
                  position: 'absolute',
                  bottom: 0,
                  left: 0,
                  width: 40,
                  height: 3,
                  bgcolor: 'primary.main'
                }
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
            >
              Додати
            </Button>
          </Box>
        )}
        
        {/* Tabs */}
        <Tabs value={tabValue} onChange={handleTabChange} sx={{ mb: 2 }}>
          <Tab label="Перегляд" />
          <Tab label="Додати інтервал" />
        </Tabs>
        
        {/* Tab panels */}
        <Box hidden={tabValue !== 0}>
          <TimeSlots />
        </Box>
        <Box hidden={tabValue !== 1}>
          <AddIntervalForm />
        </Box>
      </Box>
    </Box>
  );
};

export default Calendar;