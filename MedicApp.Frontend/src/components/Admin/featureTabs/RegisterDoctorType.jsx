import React, { useState } from 'react';
import { 
  Box, 
  Tabs, 
  Tab, 
  Typography, 
  TextField, 
  Button, 
  Autocomplete, 
  Chip, 
  Paper,
  Grid,
  Container,
  FormControl,
  InputLabel,
  OutlinedInput,
  InputAdornment,
  IconButton,
  FormHelperText,
  Divider
} from '@mui/material';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import HomeIcon from '@mui/icons-material/Home';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import SchoolIcon from '@mui/icons-material/School';
import {BASE_API} from '../../../constants/BASE_API';
// Доступні університети для вибору
const UNIVERSITIES = [
  "Kyiv Medical University",
  "Bogomolets National Medical University",
  "Lviv National Medical University",
  "Kharkiv National Medical University",
  "Danylo Halytsky Lviv National Medical University",
  "Vinnytsia National Medical University",
  "Zaporizhzhia State Medical University",
  "Odessa National Medical University",
  "Ivano-Frankivsk National Medical University",
  "Bukovinian State Medical University"
];

// Доступні спеціалізації для вибору
const SPECIALIZATIONS = [
  "Traumatologist",
  "Psychotherapist",
  "Obstetrician",
  "Therapist"
];

// TabPanel компонент для перемикання між вкладками
function TabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          {children}
        </Box>
      )}
    </div>
  );
}

function a11yProps(index) {
  return {
    id: `simple-tab-${index}`,
    'aria-controls': `simple-tabpanel-${index}`,
  };
}

const RegisterDoctorType = () => {
  // Стан для вкладок
  const [tabValue, setTabValue] = useState(0);

  // Стани для форми
  const [formData, setFormData] = useState({
    firstname: '',
    lastname: '',
    password: '',
    phonenumber: '',
    email: '',
    specialization: [],
    education: [],
    address: {
      street: '',
      building: '',
      appartaments: '',
      country: '',
      city: ''
    }
  });

  // Стани для валідації
  const [errors, setErrors] = useState({});
  const [showPassword, setShowPassword] = useState(false);

  // Обробник зміни вкладок
  const handleTabChange = (event, newValue) => {
    setTabValue(newValue);
  };

  // Обробник зміни текстових полів
  const handleChange = (e) => {
    const { name, value } = e.target;
    
    if (name.includes('.')) {
      const [parent, child] = name.split('.');
      setFormData({
        ...formData,
        [parent]: {
          ...formData[parent],
          [child]: value
        }
      });
    } else {
      setFormData({
        ...formData,
        [name]: value
      });
    }
  };

  // Обробник зміни спеціалізацій
  const handleSpecializationChange = (event, newValue) => {
    setFormData({
      ...formData,
      specialization: newValue
    });
  };

  // Обробник зміни освіти
  const handleEducationChange = (event, newValue) => {
    setFormData({
      ...formData,
      education: newValue
    });
  };

  // Обробник переключення видимості пароля
  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  // Валідація форми
  const validateForm = () => {
    const newErrors = {};
    
    // Перевірка імені
    if (!formData.firstname) {
      newErrors.firstname = "Ім'я обов'язкове";
    }
    
    // Перевірка прізвища
    if (!formData.lastname) {
      newErrors.lastname = "Прізвище обов'язкове";
    }
    
    // Перевірка пароля
    if (!formData.password) {
      newErrors.password = "Пароль обов'язковий";
    } else if (!/(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})/.test(formData.password)) {
      newErrors.password = "Пароль повинен містити мінімум 8 символів, великі та малі літери, цифри та спеціальні символи";
    }
    
    // Перевірка email
    if (!formData.email) {
      newErrors.email = "Email обов'язковий";
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = "Неправильний формат email";
    }
    
    // Перевірка телефону
    if (!formData.phonenumber) {
      newErrors.phonenumber = "Номер телефону обов'язковий";
    }
    
    // Перевірка спеціалізацій
    if (formData.specialization.length === 0) {
      newErrors.specialization = "Виберіть хоча б одну спеціалізацію";
    }
    
    // Перевірка освіти
    if (formData.education.length === 0) {
      newErrors.education = "Виберіть хоча б один навчальний заклад";
    }
    
    // Перевірка адреси
    if (!formData.address.country) {
      newErrors['address.country'] = "Країна обов'язкова";
    }
    
    if (!formData.address.city) {
      newErrors['address.city'] = "Місто обов'язкове";
    }
    
    if (!formData.address.street) {
      newErrors['address.street'] = "Вулиця обов'язкова";
    }
    
    if (!formData.address.building) {
      newErrors['address.building'] = "Номер будинку обов'язковий";
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    
      console.log("Форма відправлена:", formData);
      const token = localStorage.getItem('accessToken');
      
      try {
        const response = await fetch(`${BASE_API}/register/doctor`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          },
          body: JSON.stringify(formData)
        });
        
        if (!response.ok) {
          throw new Error(`Error: ${response.status}`);
        }
        
        const result = await response.json();
        console.log("Registration successful:", result);
        
      } catch (error) {
        console.error("Registration failed:", error);
      }
  };

  return (
    <Container maxWidth="md">
      <Paper elevation={3} sx={{ mt: 4, mb: 4, overflow: 'hidden' }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
          <Tabs 
            value={tabValue} 
            onChange={handleTabChange} 
            aria-label="registration tabs"
            variant="fullWidth"
          >
            <Tab 
              icon={<LocalHospitalIcon />} 
              label="Реєстрація лікаря" 
              {...a11yProps(0)} 
            />
            <Tab 
              icon={<AccountCircleIcon />} 
              label="Реєстрація пацієнта" 
              {...a11yProps(1)} 
              disabled
            />
          </Tabs>
        </Box>
        
        <TabPanel value={tabValue} index={0}>
          <Typography variant="h5" component="h2" gutterBottom align="center">
            Реєстрація лікаря
          </Typography>
          
          <form onSubmit={handleSubmit}>
            <Grid container spacing={3}>
              {/* Персональна інформація */}
              <Grid item xs={12}>
                <Typography variant="h6" sx={{ mb: 2, display: 'flex', alignItems: 'center' }}>
                  <AccountCircleIcon sx={{ mr: 1 }} />
                  Персональна інформація
                </Typography>
                <Grid container spacing={2}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Ім'я"
                      name="firstname"
                      value={formData.firstname}
                      onChange={handleChange}
                      error={!!errors.firstname}
                      helperText={errors.firstname}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Прізвище"
                      name="lastname"
                      value={formData.lastname}
                      onChange={handleChange}
                      error={!!errors.lastname}
                      helperText={errors.lastname}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <FormControl fullWidth variant="outlined" error={!!errors.password}>
                      <InputLabel htmlFor="password">Пароль</InputLabel>
                      <OutlinedInput
                        id="password"
                        name="password"
                        type={showPassword ? 'text' : 'password'}
                        value={formData.password}
                        onChange={handleChange}
                        endAdornment={
                          <InputAdornment position="end">
                            <IconButton
                              aria-label="toggle password visibility"
                              onClick={handleClickShowPassword}
                              edge="end"
                            >
                              {showPassword ? <VisibilityOff /> : <Visibility />}
                            </IconButton>
                          </InputAdornment>
                        }
                        label="Пароль"
                      />
                      {errors.password && <FormHelperText>{errors.password}</FormHelperText>}
                    </FormControl>
                  </Grid>
                </Grid>
              </Grid>

              <Grid item xs={12}>
                <Divider />
              </Grid>
              
              {/* Контактна інформація */}
              <Grid item xs={12}>
                <Typography variant="h6" sx={{ mb: 2, display: 'flex', alignItems: 'center' }}>
                  <PhoneIcon sx={{ mr: 1 }} />
                  Контактна інформація
                </Typography>
                <Grid container spacing={2}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Email"
                      name="email"
                      type="email"
                      value={formData.email}
                      onChange={handleChange}
                      error={!!errors.email}
                      helperText={errors.email}
                      variant="outlined"
                      InputProps={{
                        startAdornment: (
                          <InputAdornment position="start">
                            <EmailIcon />
                          </InputAdornment>
                        ),
                      }}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Номер телефону"
                      name="phonenumber"
                      value={formData.phonenumber}
                      onChange={handleChange}
                      error={!!errors.phonenumber}
                      helperText={errors.phonenumber}
                      variant="outlined"
                      InputProps={{
                        startAdornment: (
                          <InputAdornment position="start">
                            <PhoneIcon />
                          </InputAdornment>
                        ),
                      }}
                    />
                  </Grid>
                </Grid>
              </Grid>

              <Grid item xs={12}>
                <Divider />
              </Grid>
              
              {/* Професійна інформація */}
              <Grid item xs={12}>
                <Typography variant="h6" sx={{ mb: 2, display: 'flex', alignItems: 'center' }}>
                  <LocalHospitalIcon sx={{ mr: 1 }} />
                  Професійна інформація
                </Typography>
                <Grid container spacing={2}>
                  <Grid item xs={12}>
                    <Autocomplete
                      multiple
                      id="specialization"
                      options={SPECIALIZATIONS}
                      value={formData.specialization}
                      onChange={handleSpecializationChange}
                      renderInput={(params) => (
                        <TextField
                          {...params}
                          label="Спеціалізації"
                          variant="outlined"
                          error={!!errors.specialization}
                          helperText={errors.specialization}
                        />
                      )}
                      renderTags={(value, getTagProps) =>
                        value.map((option, index) => (
                          <Chip 
                            label={option} 
                            {...getTagProps({ index })} 
                            color="primary" 
                            variant="outlined"
                          />
                        ))
                      }
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <Autocomplete
                      multiple
                      id="education"
                      options={UNIVERSITIES}
                      value={formData.education}
                      onChange={handleEducationChange}
                      renderInput={(params) => (
                        <TextField
                          {...params}
                          label="Освіта"
                          variant="outlined"
                          error={!!errors.education}
                          helperText={errors.education}
                          InputProps={{
                            ...params.InputProps,
                            startAdornment: (
                              <>
                                <InputAdornment position="start">
                                  <SchoolIcon />
                                </InputAdornment>
                                {params.InputProps.startAdornment}
                              </>
                            ),
                          }}
                        />
                      )}
                      renderTags={(value, getTagProps) =>
                        value.map((option, index) => (
                          <Chip 
                            label={option} 
                            {...getTagProps({ index })} 
                            color="secondary" 
                            variant="outlined"
                          />
                        ))
                      }
                    />
                  </Grid>
                </Grid>
              </Grid>

              <Grid item xs={12}>
                <Divider />
              </Grid>
              
              {/* Адреса */}
              <Grid item xs={12}>
                <Typography variant="h6" sx={{ mb: 2, display: 'flex', alignItems: 'center' }}>
                  <HomeIcon sx={{ mr: 1 }} />
                  Адреса
                </Typography>
                <Grid container spacing={2}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Країна"
                      name="address.country"
                      value={formData.address.country}
                      onChange={handleChange}
                      error={!!errors['address.country']}
                      helperText={errors['address.country']}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Місто"
                      name="address.city"
                      value={formData.address.city}
                      onChange={handleChange}
                      error={!!errors['address.city']}
                      helperText={errors['address.city']}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Вулиця"
                      name="address.street"
                      value={formData.address.street}
                      onChange={handleChange}
                      error={!!errors['address.street']}
                      helperText={errors['address.street']}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12} sm={3}>
                    <TextField
                      fullWidth
                      label="Будинок"
                      name="address.building"
                      value={formData.address.building}
                      onChange={handleChange}
                      error={!!errors['address.building']}
                      helperText={errors['address.building']}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12} sm={3}>
                    <TextField
                      fullWidth
                      label="Квартира"
                      name="address.appartaments"
                      value={formData.address.appartaments}
                      onChange={handleChange}
                      error={!!errors['address.appartaments']}
                      helperText={errors['address.appartaments']}
                      variant="outlined"
                    />
                  </Grid>
                </Grid>
              </Grid>
              
              <Grid item xs={12}>
                <Button
                  type="submit"
                  variant="contained"
                  color="primary"
                  size="large"
                  fullWidth
                  sx={{ mt: 2 }}
                >
                  Зареєструватися
                </Button>
              </Grid>
            </Grid>
          </form>
        </TabPanel>
        
        <TabPanel value={tabValue} index={1}>
          <Typography variant="h5" gutterBottom align="center">
            Реєстрація пацієнта
          </Typography>
          <Typography variant="body1" align="center">
            Форма для реєстрації пацієнта буде реалізована в майбутньому
          </Typography>
        </TabPanel>
      </Paper>
    </Container>
  );
};

export default RegisterDoctorType;