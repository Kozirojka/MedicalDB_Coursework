import React from 'react';
import {
  Card,
  CardContent,
  Typography,
  Box,
  Button,
  Chip,
  Avatar,
  Divider
} from '@mui/material';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import PersonIcon from '@mui/icons-material/Person';
import MedicalServicesIcon from '@mui/icons-material/MedicalServices';

/**
 * Компонент для відображення картки запиту на візит
 * 
 * @param {Object} request - дані запиту
 * @param {Function} onAssignTime - функція для відкриття календаря
 */
const VisitRequestCard = ({ request, onAssignTime }) => {
  // Форматування дати у вигляді "10 травня 2023"
  const formatDate = (dateString) => {
    if (!dateString) return "Дата не вказана";
    
    const months = [
      'січня', 'лютого', 'березня', 'квітня', 'травня', 'червня',
      'липня', 'серпня', 'вересня', 'жовтня', 'листопада', 'грудня'
    ];
    
    const date = new Date(dateString);
    return `${date.getDate()} ${months[date.getMonth()]} ${date.getFullYear()}`;
  };

  return (
    <Card 
      elevation={2} 
      sx={{ 
        mb: 2, 
        borderRadius: 2,
        maxWidth: 400, // Додана максимальна ширина картки
        width: '100%', // Забезпечує адаптивність для менших екранів
        overflow: 'hidden',
        transition: 'all 0.2s ease',
        '&:hover': {
          boxShadow: 4,
          transform: 'translateY(-2px)'
        } 
      }}
    >
      <Box sx={{ display: 'flex', bgcolor: 'primary.main', p: 1, alignItems: 'center' }}>
        <Avatar sx={{ bgcolor: 'white', color: 'primary.main', mr: 1 }}>
          <PersonIcon />
        </Avatar>
        <Typography variant="subtitle1" fontWeight={500} color="white">
          {request.patientId || "Пацієнт"}
        </Typography>
      </Box>

      <CardContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5 }}>
          {/* Інформація про запит */}
          <Box sx={{ display: 'flex', alignItems: 'flex-start' }}>
            <MedicalServicesIcon sx={{ mr: 1, color: 'text.secondary' }} />
            <Box>
              <Typography variant="body2" color="text.secondary">
                Послуга
              </Typography>
              <Typography variant="body1">
                {request.serviceName || "Консультація лікаря"}
              </Typography>
            </Box>
          </Box>

          {/* Бажана дата */}
          <Box sx={{ display: 'flex', alignItems: 'flex-start' }}>
            <CalendarMonthIcon sx={{ mr: 1, color: 'text.secondary' }} />
            <Box>
              <Typography variant="body2" color="text.secondary">
                Бажана дата
              </Typography>
              <Typography variant="body1">
                {formatDate(request.dateTime)}
              </Typography>
            </Box>
          </Box>

          {/* Бажаний час */}
          <Box sx={{ display: 'flex', alignItems: 'flex-start' }}>
            <AccessTimeIcon sx={{ mr: 1, color: 'text.secondary' }} />
            <Box>
              <Typography variant="body2" color="text.secondary">
                Бажаний час
              </Typography>
              <Typography variant="body1">
                {request.dateTimeEnd || "Не вказано"}
              </Typography>
            </Box>
          </Box>

          {/* Коментар пацієнта */}
          {request.comment && (
            <Box sx={{ mt: 1 }}>
              <Divider sx={{ my: 1 }} />
              <Typography variant="body2" color="text.secondary">
                Коментар пацієнта:
              </Typography>
              <Typography variant="body2" sx={{ mt: 0.5 }}>
                {request.description}
              </Typography>
            </Box>
          )}

          <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 1 }}>
            <Button 
              variant="contained" 
              onClick={() => onAssignTime(request)}
              startIcon={<CalendarMonthIcon />}
            >
              Призначити час
            </Button>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};

export default VisitRequestCard;