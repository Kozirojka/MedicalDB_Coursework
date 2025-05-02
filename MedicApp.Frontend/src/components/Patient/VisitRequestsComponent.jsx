import { useState, useEffect } from "react";
import { 
  Card, 
  CardContent, 
  Typography, 
  Button, 
  Box, 
  Chip,
  CircularProgress,
  Drawer,
  IconButton,
  Divider,
  Avatar,
  Grid
} from "@mui/material";
import { BASE_API } from '../../constants/BASE_API';
import CloseIcon from '@mui/icons-material/Close';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import PersonIcon from '@mui/icons-material/Person';

const VisitRequestsComponent = () => {
  const [visitRequests, setVisitRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [selectedDoctor, setSelectedDoctor] = useState(null);
  const [doctorDataLoading, setDoctorDataLoading] = useState(false);

  useEffect(() => {
    fetchVisitRequests();
  }, []);

  const fetchVisitRequests = async () => {
    try {
      setLoading(true);
      const token = localStorage.getItem('accessToken'); 
      
      const response = await fetch(`${BASE_API}/patient/visit-requests`, {
        method: 'GET',
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        }
      });
      
      if (!response.ok) {
        throw new Error('Failed to fetch visit requests');
      }
      
      const data = await response.json();

      console.log(data)
      setVisitRequests(data);
    } catch (err) {
      setError(err.message);
      console.error('Error fetching visit requests:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleRejectRequest = async (requestId) => {
    try {
      const token = localStorage.getItem('accessToken');
      
      const response = await fetch(`${BASE_API}/v2/patient/appointment/assign/cancle/${requestId}`, {
        method: 'PUT',
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        }
      });
      
      if (!response.ok) {
        throw new Error('Failed to reject visit request');
      }
      
      fetchVisitRequests();
    } catch (err) {
      console.error('Error rejecting visit request:', err);
    }
  };

  const fetchDoctorInfo = async (doctorId) => {
    try {
      setDoctorDataLoading(true);
      const token = localStorage.getItem('accessToken');
      console.log(token)

      const response = await fetch(`${BASE_API}/doctor/info/${doctorId}`, {
        method: 'GET',
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        }
      });
      
      if (!response.ok) {
        throw new Error('Failed to fetch doctor information');
      }
      
      const data = await response.json();
      setSelectedDoctor(data);
    } catch (err) {
      console.error('Error fetching doctor information:', err);
    } finally {
      setDoctorDataLoading(false);
    }
  };

  const handleDoctorClick = (doctorId, doctorName) => {
    setDrawerOpen(true);
    fetchDoctorInfo(doctorId);
  };

  const formatDate = (dateString) => {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  const formatTime = (timeString) => {
    const [hours, minutes] = timeString.split(':');
    const date = new Date();
    date.setHours(parseInt(hours, 10));
    date.setMinutes(parseInt(minutes, 10));
    
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  // Status color mapping
  const statusColors = {
    "AssignedToDoctor": "warning",
    "InProgress": "info",
    "Completed": "success",
    "DoctorRejected": "error",
    "CancelledByPatient": "error"
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box sx={{ p: 2, color: 'error.main' }}>
        <Typography variant="h6">Error: {error}</Typography>
        <Button 
          variant="outlined" 
          color="primary" 
          onClick={fetchVisitRequests}
          sx={{ mt: 2 }}
        >
          Retry
        </Button>
      </Box>
    );
  }

  if (visitRequests.length === 0) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography variant="h6" align="center">No visit requests found</Typography>
      </Box>
    );
  }

  return (
    <>
      <Box sx={{ p: 2 }}>
        <Typography variant="h5" gutterBottom>Your Visit Requests</Typography>
        
        <Grid container spacing={2}>
          {visitRequests.map((request) => (
            <Grid item xs={12} sm={6} md={4} key={request.id}>
              <Card sx={{ mb: 2, height: '100%', display: 'flex', flexDirection: 'column' }}>
                <CardContent sx={{ flexGrow: 1 }}>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                    <Chip 
                      label={request.statusName} 
                      color={statusColors[request.statusName] || "default"} 
                      size="small" 
                    />
                    <Typography variant="caption" color="text.secondary">
                      ID: {request.id}
                    </Typography>
                  </Box>
                  
                  <Typography variant="body1" sx={{ mb: 2 }}>
                    {request.description}
                  </Typography>
                  
                  <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                    <PersonIcon fontSize="small" sx={{ mr: 1, color: 'primary.main' }} />
                    <Typography 
                      variant="body2" 
                      component="span"
                      sx={{ 
                        fontWeight: 'medium', 
                        color: 'primary.main',
                        cursor: 'pointer',
                        '&:hover': { textDecoration: 'underline' }
                      }}
                      onClick={() => handleDoctorClick(request.doctorId, request.doctorName)}
                    >
                      Dr. {request.doctorName}
                    </Typography>
                  </Box>
                  
                  <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                    <CalendarTodayIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                    <Typography variant="body2" color="text.secondary">
                      {formatDate(request.scheduleInfo.date)}
                    </Typography>
                  </Box>
                  
                  <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <AccessTimeIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                    <Typography variant="body2" color="text.secondary">
                      {formatTime(request.scheduleInfo.startTime)} - {formatTime(request.scheduleInfo.endTime)}
                    </Typography>
                  </Box>
                </CardContent>
                
                <Box sx={{ display: 'flex', justifyContent: 'space-between', p: 2, pt: 0 }}>
                  <Button
                    variant="outlined"
                    color="error"
                    size="small"
                    onClick={() => handleRejectRequest(request.id)}
                  >
                    Reject
                  </Button>
                  <Button
                    variant="contained"
                    color="primary"
                    size="small"
                    onClick={() => handleDoctorClick(request.doctorId, request.doctorName)}
                  >
                    Doctor Info
                  </Button>
                </Box>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>

      {/* Doctor Information Drawer */}
      <Drawer
        anchor="right"
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        PaperProps={{
          sx: { width: { xs: '100%', sm: 400 } }
        }}
      >
        <Box sx={{ p: 2 }}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
            <Typography variant="h6">Doctor Information</Typography>
            <IconButton onClick={() => setDrawerOpen(false)}>
              <CloseIcon />
            </IconButton>
          </Box>
          
          <Divider sx={{ mb: 2 }} />
          
          {doctorDataLoading ? (
            <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
              <CircularProgress />
            </Box>
          ) : selectedDoctor ? (
            <>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
                <Avatar sx={{ width: 64, height: 64, mr: 2, bgcolor: 'primary.main' }}>
                  {selectedDoctor.fullname?.charAt(0) || 'D'}
                </Avatar>
                <Box>
                  <Typography variant="h6">Dr. {selectedDoctor.fullName || 'Unknown'}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    {selectedDoctor.specialization || selectedDoctor.specialty || 'Specialist'}
                  </Typography>
                </Box>
              </Box>
              
              <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
                Professional Information
              </Typography>
              <Box sx={{ mb: 2 }}>
                <Typography variant="body2">
                  <strong>Experience:</strong> {selectedDoctor.experience || 'Not specified'}
                </Typography>
                <Typography variant="body2">
                  <strong>Education:</strong> {selectedDoctor.education || 'Not specified'}
                </Typography>
                {selectedDoctor.appointments && (
                  <Typography variant="body2">
                    <strong>Total Appointments:</strong> {selectedDoctor.appointments}
                  </Typography>
                )}
                {selectedDoctor.rating && (
                  <Typography variant="body2">
                    <strong>Rating:</strong> {selectedDoctor.rating}/5.0
                  </Typography>
                )}
              </Box>
              
              {selectedDoctor.bio && (
                <>
                  <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
                    Biography
                  </Typography>
                  <Typography variant="body2" paragraph>
                    {selectedDoctor.bio}
                  </Typography>
                </>
              )}
              
              {selectedDoctor.availability && (
                <>
                  <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
                    Availability
                  </Typography>
                  <Typography variant="body2" paragraph>
                    {selectedDoctor.availability}
                  </Typography>
                </>
              )}
              
              <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
                Contact Information
              </Typography>
              <Typography variant="body2">
                <strong>Email:</strong> {selectedDoctor.email || selectedDoctor.contactInfo?.email || 'Not available'}
              </Typography>
              <Typography variant="body2">
                <strong>Phone:</strong> {selectedDoctor.phonenumber || selectedDoctor.contactInfo?.phonenumber || 'Not available'}
              </Typography>
              
            </>
          ) : (
            <Typography>No doctor information available</Typography>
          )}
        </Box>
      </Drawer>
    </>
  );
};

export default VisitRequestsComponent;