import React, { useState, useEffect } from 'react';
import {
  Box,
  Card,
  CardContent,
  CardActions,
  Button,
  Typography,
  Drawer,
  IconButton,
  Divider,
  Avatar,
  Grid,
  Container,
  Paper,
  Chip,
  List,
  ListItem,
  ListItemText,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Stack,
  Alert
} from '@mui/material';

import { BASE_API } from '../../../constants/BASE_API';

import CloseIcon from '@mui/icons-material/Close';
import PersonIcon from '@mui/icons-material/Person';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import CommentIcon from '@mui/icons-material/Comment';
import MedicalServicesIcon from '@mui/icons-material/MedicalServices';
import CircularProgress from '@mui/material/CircularProgress';

// Helper function to format dates
const formatDate = (dateString) => {
  if (!dateString) return 'N/A';
  return new Date(dateString).toLocaleString();
};

// Status color mapping
const statusColors = {
  'Pending': 'warning',
  'Approved': 'primary',
  'Rejected': 'error',
  'InProgress': 'info',
  'Completed': 'success',
  'CancelledByPatient': 'error',
  'CancelledByDoctor': 'error'
};

export default function TabPatientDetail() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedUser, setSelectedUser] = useState(null);
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [patientHistory, setPatientHistory] = useState(null);
  const [historyLoading, setHistoryLoading] = useState(false);
  const [historyError, setHistoryError] = useState(null);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    setLoading(true);
    try {
      console.log(`${BASE_API}/doctor/patients`);
      const response = await fetch(`${BASE_API}/doctor/patients`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
        },
      });

      if (!response.ok) {
        throw new Error('Помилка завантаження даних');
      }
      
      const data = await response.json();
      console.log(data);
      
      if (Array.isArray(data)) {
        setUsers(data);
      } else {
        setUsers([data]);
      }
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // Fetch patient history
  const fetchPatientHistory = async (patientId) => {
    setHistoryLoading(true);
    setHistoryError(null);
    try {
      const response = await fetch(`${BASE_API}/doctor/patient/history?id=${patientId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
        },
      });

      if (!response.ok) {
        throw new Error('Помилка завантаження історії пацієнта');
      }
      
      const data = await response.json();
      console.log("Patient history:", data);
      setPatientHistory(data);
    } catch (err) {
      console.error("Error fetching patient history:", err);
      setHistoryError(err.message);
    } finally {
      setHistoryLoading(false);
    }
  };

  const handleViewDetails = async (user) => {
    setSelectedUser(user);
    setDrawerOpen(true);
    await fetchPatientHistory(user.id);
  };

  const handleCloseDrawer = () => {
    setDrawerOpen(false);
    setPatientHistory(null);
  };

  // Функція для отримання повної адреси з об'єкта адреси
  const getFullAddress = (address) => {
    if (!address) return '';
    return `${address.country}, ${address.city}, ${address.street} ${address.building}, кв. ${address.appartaments}`;
  };

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" gutterBottom>
        Інформація про пацієнтів
      </Typography>

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
          <CircularProgress />
        </Box>
      ) : error ? (
        <Typography color="error" align="center">
          {error}
        </Typography>
      ) : (
        <Grid container spacing={3}>
          {users.map((user) => (
            <Grid item key={user.id} xs={12} sm={6} md={4}>
              <Card 
                sx={{ 
                  height: '100%', 
                  display: 'flex', 
                  flexDirection: 'column',
                  boxShadow: 3,
                  '&:hover': {
                    boxShadow: 6,
                  },
                }}
              >
                <CardContent sx={{ flexGrow: 1 }}>
                  <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                    <Avatar 
                      sx={{ 
                        bgcolor: 'secondary.main',
                        mr: 2
                      }}
                    >
                      <PersonIcon />
                    </Avatar>
                    <Typography variant="h6" component="div">
                      {`${user.firstName} ${user.lastName}`}
                    </Typography>
                  </Box>
                  
                  <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                    <EmailIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                    <Typography variant="body2" color="text.secondary">
                      {user.email}
                    </Typography>
                  </Box>
                  
                  <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                    <PhoneIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                    <Typography variant="body2" color="text.secondary">
                      {user.phoneNumber}
                    </Typography>
                  </Box>
                  
                  {user.addresses && user.addresses.length > 0 && (
                    <Box sx={{ display: 'flex', alignItems: 'flex-start', mb: 1 }}>
                      <LocationOnIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary', mt: 0.5 }} />
                      <Typography variant="body2" color="text.secondary">
                        {user.addresses[0].city}, {user.addresses[0].country}
                      </Typography>
                    </Box>
                  )}
                </CardContent>
                <CardActions>
                  <Button 
                    size="medium" 
                    color="primary" 
                    onClick={() => handleViewDetails(user)}
                    fullWidth
                  >
                    Дізнатись більше
                  </Button>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}
      
      {/* Детальна інформація користувача в Drawer */}
      <Drawer
        anchor="right"
        open={drawerOpen}
        onClose={handleCloseDrawer}
        PaperProps={{
          sx: { width: { xs: '100%', sm: 700, md: 900 }, overflowY: 'auto' }
        }}
      >
        {selectedUser && (
          <Box sx={{ p: 3 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
              <Typography variant="h6">Інформація про пацієнта</Typography>
              <IconButton onClick={handleCloseDrawer}>
                <CloseIcon />
              </IconButton>
            </Box>
            
            <Divider sx={{ mb: 3 }} />
            
            {/* Basic Patient Info */}
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
              <Avatar 
                sx={{ 
                  width: 64, 
                  height: 64, 
                  mr: 2, 
                  bgcolor: 'secondary.main' 
                }}
              >
                <PersonIcon />
              </Avatar>
              <Box>
                <Typography variant="h6">
                  {patientHistory 
                    ? patientHistory.accountInfo.fullName 
                    : `${selectedUser.firstName} ${selectedUser.lastName}`}
                </Typography>
                <Chip 
                  label="Пацієнт" 
                  color="secondary" 
                  size="small" 
                />
              </Box>
            </Box>
            
            {historyLoading ? (
              <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
                <CircularProgress />
              </Box>
            ) : historyError ? (
              <Alert severity="error" sx={{ mb: 3 }}>
                {historyError}
              </Alert>
            ) : patientHistory ? (
              <>
                {/* Contact Information */}
                <Paper sx={{ p: 2, mb: 3 }} elevation={2}>
                  <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
                    Контактна інформація
                  </Typography>
                  <Grid container spacing={2}>
                    <Grid item xs={12} sm={6}>
                      <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                        <EmailIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                        <Typography variant="body2">{patientHistory.accountInfo.email}</Typography>
                      </Box>
                    </Grid>
                    <Grid item xs={12} sm={6}>
                      <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                        <PhoneIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                        <Typography variant="body2">{patientHistory.accountInfo.phoneNumber}</Typography>
                      </Box>
                    </Grid>
                    <Grid item xs={12}>
                      <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                        <CalendarTodayIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                        <Typography variant="body2">
                          Дата реєстрації: {formatDate(patientHistory.accountInfo.createdAt)}
                        </Typography>
                      </Box>
                    </Grid>
                  </Grid>
                </Paper>

                {/* Addresses */}
                {patientHistory.accountInfo.addresses && patientHistory.accountInfo.addresses.length > 0 && (
                  <Paper sx={{ p: 2, mb: 3 }} elevation={2}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
                      Адреси
                    </Typography>
                    <List dense>
                      {patientHistory.accountInfo.addresses.map((address, index) => (
                        <ListItem key={index}>
                          <LocationOnIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                          <ListItemText 
                            primary={getFullAddress(address)} 
                            secondary={address.region} 
                          />
                        </ListItem>
                      ))}
                    </List>
                  </Paper>
                )}

                {/* Medical Help Requests */}
                <Typography variant="h6" sx={{ mb: 2, mt: 3 }}>
                  Історія медичних запитів 
                  <Chip 
                    label={patientHistory.medicalRequests.length} 
                    size="small" 
                    color="primary" 
                    sx={{ ml: 1 }} 
                  />
                </Typography>

                {patientHistory.medicalRequests.length === 0 ? (
                  <Alert severity="info">Пацієнт не має історії медичних запитів</Alert>
                ) : (
                  patientHistory.medicalRequests.map((request) => (
                    <Accordion key={request.id} sx={{ mb: 2 }}>
                      <AccordionSummary
                        expandIcon={<ExpandMoreIcon />}
                        aria-controls={`panel-${request.id}-content`}
                        id={`panel-${request.id}-header`}
                        sx={{ borderLeft: 4, borderColor: `${statusColors[request.status.name]}.main` }}
                      >
                        <Grid container alignItems="center" spacing={1}>
                          <Grid item>
                            <MedicalServicesIcon color={statusColors[request.status.name]} />
                          </Grid>
                          <Grid item xs>
                            <Typography variant="subtitle1">
                              Запит №{request.id}
                            </Typography>
                          </Grid>
                          <Grid item>
                            <Chip 
                              label={request.status.name} 
                              color={statusColors[request.status.name]}
                              size="small"
                            />
                          </Grid>
                          <Grid item xs={12}>
                            <Typography variant="caption" color="text.secondary">
                              Створено: {formatDate(request.createdAt)}
                            </Typography>
                          </Grid>
                        </Grid>
                      </AccordionSummary>
                      <AccordionDetails>
                        <Stack spacing={2}>
                          <Paper sx={{ p: 2, bgcolor: 'background.default' }}>
                            <Typography variant="subtitle2" gutterBottom>Опис запиту:</Typography>
                            <Typography variant="body2">{request.description}</Typography>
                          </Paper>

                          {/* Schedule Info if available */}
                          {request.scheduleInfo && (
                            <Paper sx={{ p: 2, bgcolor: 'background.default' }}>
                              <Typography variant="subtitle2" gutterBottom>Інформація про призначення:</Typography>
                              <Typography variant="body2">
                                Дата: {request.scheduleInfo.date ? new Date(request.scheduleInfo.date).toLocaleDateString() : 'Не призначено'}
                              </Typography>
                              {request.scheduleInfo.startTime && request.scheduleInfo.endTime && (
                                <Typography variant="body2">
                                  Час: {request.scheduleInfo.startTime} - {request.scheduleInfo.endTime}
                                </Typography>
                              )}
                            </Paper>
                          )}

                          {/* Comments */}
                          <Typography variant="subtitle2">
                            Коментарі {request.comments.length > 0 && `(${request.comments.length})`}:
                          </Typography>
                          
                          {request.comments.length === 0 ? (
                            <Typography variant="body2" color="text.secondary">Немає коментарів</Typography>
                          ) : (
                            <List dense>
                              {request.comments.map((comment) => (
                                <ListItem key={comment.id} sx={{ 
                                  mb: 1, 
                                  p: 2, 
                                  bgcolor: 'background.default',
                                  borderRadius: 1 
                                }}>
                                  <Stack spacing={1} width="100%">
                                    <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                                      <Typography variant="body2" sx={{ fontWeight: 'bold' }}>
                                        <CommentIcon fontSize="small" sx={{ mr: 0.5, verticalAlign: 'middle' }} />
                                        {comment.authorName}
                                      </Typography>
                                      <Typography variant="caption" color="text.secondary">
                                        {formatDate(comment.createdAt)}
                                      </Typography>
                                    </Box>
                                    <Typography variant="body2">{comment.text}</Typography>
                                    {comment.adequacy !== null && (
                                      <Typography variant="caption" color="text.secondary">
                                        Рівень адекватності: {comment.adequacy}
                                      </Typography>
                                    )}
                                  </Stack>
                                </ListItem>
                              ))}
                            </List>
                          )}
                        </Stack>
                      </AccordionDetails>
                    </Accordion>
                  ))
                )}
              </>
            ) : (
              <Typography color="text.secondary" align="center">
                Завантаження інформації про пацієнта...
              </Typography>
            )}

            <Divider sx={{ my: 3 }} />

            <Button 
              variant="contained" 
              color="primary" 
              fullWidth
              sx={{ mt: 2 }}
            >
              Зв'язатися з пацієнтом
            </Button>
            
            <Button 
              variant="contained" 
              color="error" 
              fullWidth
              sx={{ mt: 2 }}
            >
              Заблокувати Користувача
            </Button>
          </Box>
        )}
      </Drawer>
    </Container>
  );
}