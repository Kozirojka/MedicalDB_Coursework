import React, { useState, useEffect } from 'react';
import { 
  Box, 
  Card, 
  CardContent, 
  CardActions, 
  Button, 
  Typography, 
  TextField, 
  Drawer, 
  IconButton, 
  Divider, 
  Avatar, 
  Grid, 
  FormControl, 
  InputLabel, 
  Select, 
  MenuItem, 
  Container, 
  Paper, 
  Chip
} from '@mui/material';


import {BASE_API} from '../../../constants/BASE_API';

import SearchIcon from '@mui/icons-material/Search';
import CloseIcon from '@mui/icons-material/Close';
import PersonIcon from '@mui/icons-material/Person';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import InputAdornment from '@mui/material/InputAdornment';
import CircularProgress from '@mui/material/CircularProgress';

export default function UsersTab() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedUser, setSelectedUser] = useState(null);
  const [drawerOpen, setDrawerOpen] = useState(false);
  
  const [searchQuery, setSearchQuery] = useState('');
  const [userType, setUserType] = useState('all');

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async (query = '') => {
    setLoading(true);
    try {
      const response = await fetch(`${BASE_API}/users${query}`);
      
      if (!response.ok) {
        throw new Error('Помилка завантаження даних');
      }
      
      const data = await response.json();
      
      const allUsers = [...(data.doctors || []), ...(data.patients || [])];
      setUsers(allUsers);
    } catch (err) {
      setError(err.message);
      const mockData = {
        doctors: [
          {
            id: 2,
            accountId: 14,
            fullName: "doctor1 doctor",
            phoneNumber: "+10001111",
            email: "doctor1@gmail.com",
            specializations: ["Traumatologist", "Psychotherapist"],
            type: "Doctor",
            address: {
              country: "Ukraine",
              city: "Lviv",
              street: "Akademika Andriya Sakharova St",
              building: "25",
              appartaments: "243",
              fullAddress: "Ukraine, Lviv, Akademika Andriya Sakharova St, 25, 243"
            }
          }
        ],
        patients: [
          {
            id: 4,
            accountId: 11,
            fullName: "Kostya Vykhovanets",
            phoneNumber: "+1000000000",
            email: "Kostya@gmail.com",
            type: "Patient",
            address: {
              country: "Ukraine",
              city: "Lviv",
              street: "Naukova",
              building: "4",
              appartaments: "39",
              fullAddress: "Ukraine, Lviv, Naukova, 4, 39"
            }
          },
          {
            id: 5,
            accountId: 12,
            fullName: "Artem Vykhovanets",
            phoneNumber: "+1000000001",
            email: "Artem@gmail.com",
            type: "Patient",
            address: {
              country: "Ukraine",
              city: "Lviv",
              street: "Panasa Myrnoho St",
              building: "18",
              appartaments: "39",
              fullAddress: "Ukraine, Lviv, Panasa Myrnoho St, 18, 39"
            }
          }
        ]
      };
      
      const allUsers = [...(mockData.doctors || []), ...(mockData.patients || [])];
      setUsers(allUsers);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = () => {
    let query = '';
    
    if (userType !== 'all') {
      query += `?type=${userType.toLowerCase()}`;
    }
    
    if (searchQuery) {
      query += `${query ? '&' : '?'}fullname=${encodeURIComponent(searchQuery)}`;
    }
    
    fetchUsers(query);
  };

  const handleViewDetails = (user) => {
    setSelectedUser(user);
    setDrawerOpen(true);
  };

  const handleCloseDrawer = () => {
    setDrawerOpen(false);
  };

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" gutterBottom>
        Інформація про користувачів
      </Typography>
      
      <Paper sx={{ p: 2, mb: 3 }}>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={5}>
            <TextField
              fullWidth
              variant="outlined"
              label="Пошук за ім'ям"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
              }}
            />
          </Grid>
          <Grid item xs={12} md={5}>
            <FormControl fullWidth variant="outlined">
              <InputLabel id="user-type-select-label">Тип користувача</InputLabel>
              <Select
                labelId="user-type-select-label"
                id="user-type-select"
                value={userType}
                onChange={(e) => setUserType(e.target.value)}
                label="Тип користувача"
              >
                <MenuItem value="all">Всі користувачі</MenuItem>
                <MenuItem value="doctor">Лікарі</MenuItem>
                <MenuItem value="patient">Пацієнти</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} md={2}>
            <Button 
              fullWidth 
              variant="contained" 
              color="primary" 
              onClick={handleSearch}
              sx={{ height: '56px' }}
            >
              Пошук
            </Button>
          </Grid>
        </Grid>
      </Paper>
      
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
                        bgcolor: user.type === 'Doctor' ? 'primary.main' : 'secondary.main',
                        mr: 2
                      }}
                    >
                      {user.type === 'Doctor' ? <LocalHospitalIcon /> : <PersonIcon />}
                    </Avatar>
                    <Typography variant="h6" component="div">
                      {user.fullName}
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
                  
                  <Box sx={{ display: 'flex', alignItems: 'flex-start', mb: 1 }}>
                    <LocationOnIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary', mt: 0.5 }} />
                    <Typography variant="body2" color="text.secondary">
                      {user.address?.city}, {user.address?.country}
                    </Typography>
                  </Box>
                  
                  {user.type === 'Doctor' && user.specializations && (
                    <Box sx={{ mt: 2 }}>
                      {user.specializations.map((spec, index) => (
                        <Chip 
                          key={index} 
                          label={spec} 
                          size="small" 
                          color="primary" 
                          variant="outlined"
                          sx={{ mr: 0.5, mb: 0.5 }}
                        />
                      ))}
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
          sx: { width: { xs: '100%', sm: 400 } }
        }}
      >
        {selectedUser && (
          <Box sx={{ p: 3 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
              <Typography variant="h6">Інформація про користувача</Typography>
              <IconButton onClick={handleCloseDrawer}>
                <CloseIcon />
              </IconButton>
            </Box>
            
            <Divider sx={{ mb: 3 }} />
            
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
              <Avatar 
                sx={{ 
                  width: 64, 
                  height: 64, 
                  mr: 2, 
                  bgcolor: selectedUser.type === 'Doctor' ? 'primary.main' : 'secondary.main' 
                }}
              >
                {selectedUser.type === 'Doctor' ? <LocalHospitalIcon /> : <PersonIcon />}
              </Avatar>
              <Box>
                <Typography variant="h6">{selectedUser.fullName}</Typography>
                <Chip 
                  label={selectedUser.type} 
                  color={selectedUser.type === 'Doctor' ? 'primary' : 'secondary'} 
                  size="small" 
                />
              </Box>
            </Box>
            
            <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
              Контактна інформація
            </Typography>
            <Box sx={{ mb: 3 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                <EmailIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                <Typography variant="body2">{selectedUser.email}</Typography>
              </Box>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                <PhoneIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                <Typography variant="body2">{selectedUser.phoneNumber}</Typography>
              </Box>
            </Box>
            
            <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
              Адреса
            </Typography>
            <Box sx={{ mb: 3 }}>
              <Typography variant="body2">{selectedUser.address?.fullAddress}</Typography>
            </Box>
            
            {selectedUser.type === 'Doctor' && selectedUser.specializations && (
              <>
                <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
                  Спеціалізації
                </Typography>
                <Box sx={{ mb: 3 }}>
                  {selectedUser.specializations.map((spec, index) => (
                    <Chip 
                      key={index} 
                      label={spec} 
                      size="small" 
                      color="primary" 
                      sx={{ mr: 0.5, mb: 0.5 }}
                    />
                  ))}
                </Box>
              </>
            )}
            
            <Button 
              variant="contained" 
              color="primary" 
              fullWidth
              sx={{ mt: 2 }}
            >
              {selectedUser.type === 'Doctor' ? 'Записатися на прийом' : 'Зв\'язатися з пацієнтом'}
            </Button>
          </Box>
        )}
      </Drawer>
    </Container>
  );
}