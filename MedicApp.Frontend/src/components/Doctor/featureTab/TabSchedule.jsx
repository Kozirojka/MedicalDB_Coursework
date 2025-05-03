import { useState } from 'react';
import { 
  Box, 
  Drawer, 
  Typography, 
  IconButton, 
  Divider, 
  List, 
  ListItem, 
  ListItemText, 
  Paper, 
  Chip,
  Avatar,
  Skeleton,
  Grid,
  Button,
  TextField,
  Slider,
  Snackbar,
  Alert
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import PersonIcon from '@mui/icons-material/Person';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import SaveIcon from '@mui/icons-material/Save';
import Calendar from "../../Calendar_3/Calendar";
import { BASE_API } from "../../../constants/BASE_API";

const drawerWidth = 340;

const TabSchedule = () => {
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [notesDrawerOpen, setNotesDrawerOpen] = useState(false);
  const [patientData, setPatientData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [selectedTime, setSelectedTime] = useState(null);
  const [doctorNotes, setDoctorNotes] = useState('');
  const [patientAdequacy, setPatientAdequacy] = useState(5);
  const [submitting, setSubmitting] = useState(false);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');
  const [snackbarSeverity, setSnackbarSeverity] = useState('success');

  const handleTimeSelect = async (timeId) => {
    setSelectedTime(timeId);
    setLoading(true);
    setError(null);
    
    try {
      const response = await fetch(`${BASE_API}/patinet/interval/${timeId}`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
          "Content-Type": "application/json",
        },
      });
      
      if (!response.ok) {
        throw new Error(`Error: ${response.status} ${response.statusText}`);
      }
      
      const data = await response.json();
      console.log("Fetched patient data:", data);
      setPatientData(data);
      setDrawerOpen(true);
    } catch (err) {
      console.error("Error fetching patient data:", err);
      setError(err.message || "Failed to fetch patient data");
    } finally {
      setLoading(false);
    }
  };

  const handleCloseDrawer = () => {
    setDrawerOpen(false);
  };

  const handleCloseNotesDrawer = () => {
    setNotesDrawerOpen(false);
  };

  // Rendered patient address
  const renderAddress = (address) => {
    if (!address) return "Адреса не вказана";
    
    const parts = [
      address.country,
      address.city,
      address.street,
      address.buildingNumber && `будинок ${address.buildingNumber}`,
      address.apartmentNumber && `кв. ${address.apartmentNumber}`
    ].filter(Boolean);
    
    return parts.join(", ");
  };

  const handleCompleteAppointment = () => {
    setNotesDrawerOpen(true);
  };

  const handleNotesChange = (event) => {
    setDoctorNotes(event.target.value);
  };

  const handleAdequacyChange = (event, newValue) => {
    setPatientAdequacy(newValue);
  };

  const handleSubmitNotes = async () => {
    if (!selectedTime) {
      setSnackbarMessage('Помилка: Не вибрано часовий інтервал');
      setSnackbarSeverity('error');
      setSnackbarOpen(true);
      return;
    }

    setSubmitting(true);

    try {
      // Приклад відправки даних на сервер
      console.log(selectedTime);

      const response = await fetch(`${BASE_API}/doctor/appointment/assign/complete/${selectedTime}`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          doctorNotes: doctorNotes,
          patientAdequacy: patientAdequacy
        })
      });

      if (!response.ok) {
        throw new Error(`Error: ${response.status} ${response.statusText}`);
      }

      // Успішне відправлення
      setSnackbarMessage('Нотатки про прийом збережено успішно');
      setSnackbarSeverity('success');
      setSnackbarOpen(true);
      
      // Закриваємо обидва drawer після успішного відправлення
      setNotesDrawerOpen(false);
      setDrawerOpen(false);
      
      // Скидаємо форму
      setDoctorNotes('');
      setPatientAdequacy(5);

    } catch (err) {
      console.error("Error submitting appointment notes:", err);
      setSnackbarMessage(`Помилка: ${err.message || "Не вдалося зберегти нотатки"}`);
      setSnackbarSeverity('error');
      setSnackbarOpen(true);
    } finally {
      setSubmitting(false);
    }
  };

  const handleCloseSnackbar = (event, reason) => {
    if (reason === 'clickaway') {
      return;
    }
    setSnackbarOpen(false);
  };

  const marks = [
    { value: 1, label: '1' },
    { value: 2, label: '2' },
    { value: 3, label: '3' },
    { value: 4, label: '4' },
    { value: 5, label: '5' },
    { value: 6, label: '6' },
    { value: 7, label: '7' },
    { value: 8, label: '8' },
    { value: 9, label: '9' },
    { value: 10, label: '10' },
  ];

  return (
    <Box sx={{ 
      display: 'flex', 
      justifyContent: 'center', 
      position: 'relative',
      height: '100%',
      width: '100%'
    }}>
      {/* Calendar container with transition effect */}
      <Box 
        sx={{ 
          width: { xs: '100%', sm: '90%', md: '80%', lg: '70%' },
          maxWidth: '900px',
          transition: 'margin 0.3s ease-in-out',
          marginRight: drawerOpen ? `${drawerWidth/2}px` : 0,
          height: '100%',
          display: 'flex'
        }}
      >
        <Paper 
          elevation={3} 
          sx={{ 
            width: '50%', 
            height: '50%', 
            overflow: 'hidden',
            borderRadius: 2
          }}
        >
          <Calendar onTimeSelect={handleTimeSelect} />
        </Paper>
      </Box>

      {/* Patient Data Drawer */}
      <Drawer
        anchor="right"
        open={drawerOpen}
        onClose={handleCloseDrawer}
        variant="persistent"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
            borderRadius: '0 0 0 12px',
            bgcolor: 'background.default'
          },
        }}
      >
        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', px: 2, py: 1 }}>
          <Typography variant="h6" sx={{ fontWeight: 500 }}>
            Інформація про пацієнта
          </Typography>
          <IconButton onClick={handleCloseDrawer} size="small">
            <CloseIcon />
          </IconButton>
        </Box>
        
        <Divider />
        
        {loading ? (
          <Box sx={{ p: 3 }}>
            <Skeleton variant="circular" width={80} height={80} sx={{ mb: 2, mx: 'auto' }} />
            <Skeleton variant="text" height={40} sx={{ mb: 1 }} />
            <Skeleton variant="text" height={30} sx={{ mb: 1 }} />
            <Skeleton variant="text" height={30} sx={{ mb: 1 }} />
            <Skeleton variant="rectangular" height={100} sx={{ mb: 2, borderRadius: 1 }} />
            <Skeleton variant="rectangular" height={60} sx={{ borderRadius: 1 }} />
          </Box>
        ) : error ? (
          <Box sx={{ p: 3 }}>
            <Typography color="error" align="center" sx={{ mb: 2 }}>
              {error}
            </Typography>
            <Button 
              fullWidth 
              variant="outlined" 
              color="primary" 
              onClick={handleCloseDrawer}
            >
              Закрити
            </Button>
          </Box>
        ) : patientData ? (
          <Box sx={{ p: 3 }}>
            {/* Patient Avatar and Name */}
            <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', mb: 3 }}>
              <Avatar 
                sx={{ 
                  width: 80, 
                  height: 80, 
                  bgcolor: 'primary.main',
                  mb: 2,
                  fontSize: '2rem'
                }}
              >
                {patientData.firstName ? patientData.firstName.charAt(0) : <PersonIcon fontSize="large" />}
              </Avatar>
              
              <Typography variant="h5" sx={{ fontWeight: 500, textAlign: 'center' }}>
                {`${patientData.firstName || ''} ${patientData.lastName || ''}`}
              </Typography>
              
              <Chip 
                label={`ID: ${patientData.id || 'Невідомо'}`} 
                variant="outlined" 
                size="small" 
                sx={{ mt: 1 }}
              />
            </Box>
            
            <Divider sx={{ my: 2 }} />
            
            {/* Contact Info */}
            <Grid container spacing={2} sx={{ mb: 3 }}>
              <Grid item xs={12}>
                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                  <PhoneIcon sx={{ color: 'primary.main', mr: 1 }} />
                  <Typography variant="body1">
                    {patientData.phoneNumber || 'Телефон не вказаний'}
                  </Typography>
                </Box>
              </Grid>
              
              <Grid item xs={12}>
                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                  <EmailIcon sx={{ color: 'primary.main', mr: 1 }} />
                  <Typography variant="body1">
                    {patientData.email || 'Email не вказаний'}
                  </Typography>
                </Box>
              </Grid>
            </Grid>
            
            {/* Address Info */}
            <Paper variant="outlined" sx={{ p: 2, borderRadius: 2, mb: 3 }}>
              <Typography variant="subtitle2" sx={{ mb: 1, color: 'text.secondary' }}>
                Адреса:
              </Typography>
              
              {patientData.addresses && patientData.addresses.length > 0 ? (
                <List dense disablePadding>
                  {patientData.addresses.map((address, idx) => (
                    <ListItem key={idx} disablePadding sx={{ mb: 1 }}>
                      <LocationOnIcon sx={{ color: 'primary.main', mr: 1, fontSize: '1.2rem' }} />
                      <ListItemText 
                        primary={renderAddress(address)}
                        primaryTypographyProps={{ variant: 'body2' }}
                      />
                    </ListItem>
                  ))}
                </List>
              ) : (
                <Typography variant="body2" color="text.secondary" sx={{ fontStyle: 'italic' }}>
                  Адреса не вказана
                </Typography>
              )}
            </Paper>
            
            {/* Selected Time Info */}
            {selectedTime && (
              <Paper 
                sx={{ 
                  p: 2, 
                  bgcolor: 'primary.main', 
                  color: 'primary.contrastText',
                  borderRadius: 2,
                  mb: 3
                }}
              >
                <Typography variant="subtitle2" sx={{ mb: 1 }}>
                  Обраний час прийому:
                </Typography>
                <Typography variant="body1" sx={{ fontWeight: 500 }}>
                  Інтервал ID: {selectedTime}
                </Typography>
              </Paper>
            )}

            <Button 
              variant="contained" 
              color="primary" 
              fullWidth 
              onClick={handleCompleteAppointment}
              startIcon={<SaveIcon />}
            >
              Завершити прийом
            </Button>
          </Box>
        ) : (
          <Box sx={{ p: 3, textAlign: 'center' }}>
            <Typography color="textSecondary">
              Виберіть часовий інтервал, щоб побачити дані пацієнта
            </Typography>
          </Box>
        )}
      </Drawer>

      {/* Doctor Notes Drawer */}
      <Drawer
        anchor="right"
        open={notesDrawerOpen}
        onClose={handleCloseNotesDrawer}
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
            borderRadius: '0 0 0 12px',
            bgcolor: 'background.default'
          },
        }}
      >
        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', px: 2, py: 1 }}>
          <Typography variant="h6" sx={{ fontWeight: 500 }}>
            Нотатки про прийом
          </Typography>
          <IconButton onClick={handleCloseNotesDrawer} size="small">
            <CloseIcon />
          </IconButton>
        </Box>
        
        <Divider />

        <Box sx={{ p: 3 }}>
          {patientData && (
            <Box sx={{ mb: 3 }}>
              <Typography variant="subtitle1" sx={{ fontWeight: 500 }}>
                Пацієнт: {`${patientData.firstName || ''} ${patientData.lastName || ''}`}
              </Typography>
              {selectedTime && (
                <Typography variant="body2" color="text.secondary">
                  Інтервал ID: {selectedTime}
                </Typography>
              )}
            </Box>
          )}

          <Typography variant="subtitle2" sx={{ mb: 1 }}>
            Нотатки лікаря:
          </Typography>
          <TextField
            fullWidth
            multiline
            rows={8}
            placeholder="Введіть ваші нотатки про прийом..."
            value={doctorNotes}
            onChange={handleNotesChange}
            variant="outlined"
            sx={{ mb: 3 }}
          />

          <Typography variant="subtitle2" sx={{ mb: 1 }}>
            Оцінка адекватності пацієнта (від 1 до 10):
          </Typography>
          <Box sx={{ px: 2, mb: 4 }}>
            <Slider
              value={patientAdequacy}
              onChange={handleAdequacyChange}
              step={1}
              marks={marks}
              min={1}
              max={10}
              valueLabelDisplay="auto"
            />
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 1 }}>
              <Typography variant="caption" color="text.secondary">
                Низька
              </Typography>
              <Typography variant="caption" color="text.secondary">
                Висока
              </Typography>
            </Box>
          </Box>

          <Button
            variant="contained"
            color="primary"
            fullWidth
            onClick={handleSubmitNotes}
            startIcon={<SaveIcon />}
            disabled={submitting}
          >
            {submitting ? 'Збереження...' : 'Зберегти нотатки'}
          </Button>
        </Box>
      </Drawer>

      {/* Snackbar для сповіщень */}
      <Snackbar 
        open={snackbarOpen} 
        autoHideDuration={6000} 
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      >
        <Alert 
          onClose={handleCloseSnackbar} 
          severity={snackbarSeverity} 
          sx={{ width: '100%' }}
        >
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default TabSchedule;