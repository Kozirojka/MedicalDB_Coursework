import React from 'react';
import {
  Card,
  CardContent,
  Typography,
  Box,
  Button,
  Avatar,
  Divider
} from '@mui/material';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import PersonIcon from '@mui/icons-material/Person';
import PlaceIcon from '@mui/icons-material/Place';

/**
 * Компонент для відображення картки запиту на візит
 * 
 * @param {Object} request - дані запиту
 * @param {Function} onAssignTime - функція для відкриття календаря
 */
const VisitRequestCard = ({ request, onAssignTime }) => {

  const formatDate = (dateString) => {
    if (!dateString) return "Дата не вказана";

    const months = [
      'січня', 'лютого', 'березня', 'квітня', 'травня', 'червня',
      'липня', 'серпня', 'вересня', 'жовтня', 'листопада', 'грудня'
    ];

    const date = new Date(dateString);
    return `${date.getDate()} ${months[date.getMonth()]} ${date.getFullYear()}`;
  };

  const formatTime = (dateString) => {
    if (!dateString) return "Не вказано";
    const date = new Date(dateString);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  const formatAddress = (address) => {
    if (!address) return "Адреса не вказана";
    return `${address.street}, ${address.building}, кв. ${address.appartaments}, ${address.city}, ${address.country}`;
  };

  return (
    <Card 
      elevation={2} 
      sx={{ 
        mb: 2, 
        borderRadius: 2,
        maxWidth: 400,
        width: '100%',
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
          ID пацієнта: {request.patientId || "Невідомо"}
        </Typography>
      </Box>

      <CardContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5 }}>

          {/* Статус */}
          <Box>
            <Typography variant="body2" color="text.secondary">Статус:</Typography>
            <Typography variant="body1">{request.status}</Typography>
          </Box>

          {/* Бажана дата */}
          <Box sx={{ display: 'flex', alignItems: 'flex-start' }}>
            <CalendarMonthIcon sx={{ mr: 1, color: 'text.secondary' }} />
            <Box>
              <Typography variant="body2" color="text.secondary">Бажана дата</Typography>
              <Typography variant="body1">
                {formatDate(request.dateTime)}
              </Typography>
            </Box>
          </Box>

          {/* Бажаний час */}
          <Box sx={{ display: 'flex', alignItems: 'flex-start' }}>
            <AccessTimeIcon sx={{ mr: 1, color: 'text.secondary' }} />
            <Box>
              <Typography variant="body2" color="text.secondary">Бажаний час</Typography>
              <Typography variant="body1">
                {formatTime(request.endDateTime)}
              </Typography>
            </Box>
          </Box>

          {/* Адреса */}
          <Box sx={{ display: 'flex', alignItems: 'flex-start' }}>
            <PlaceIcon sx={{ mr: 1, color: 'text.secondary' }} />
            <Box>
              <Typography variant="body2" color="text.secondary">Адреса</Typography>
              <Typography variant="body1">
                {formatAddress(request.address)}
              </Typography>
            </Box>
          </Box>

          {/* Коментар пацієнта */}
          {request.description && (
            <Box sx={{ mt: 1 }}>
              <Divider sx={{ my: 1 }} />
              <Typography variant="body2" color="text.secondary">Коментар пацієнта:</Typography>
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
