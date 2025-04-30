import VisitRequestCard from "../VisitRequestCard";
import { useState } from "react";
import { BASE_API } from '../../../constants/BASE_API';
import Calendar from '../../Calendar_2/Calendar';
import { Drawer, Typography, Box, CircularProgress, Alert } from '@mui/material';

export default function RequestForConfirm({ requests, loading, error }) {
  const [showCalendarModal, setShowCalendarModal] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState(null);
  const [loadingAssign, setLoadingAssign] = useState(false);
  
  const handleOpenCalendar = (request) => {
    setSelectedRequest(request);
    setShowCalendarModal(true);
  };
  
  const handleCloseCalendar = () => {
    setShowCalendarModal(false);
    setSelectedRequest(null);
  };
  
  const handleTimeSelect = async (timeSlotId) => {
    if (!selectedRequest) return;
    
    try {
      setLoadingAssign(true);
      const token = localStorage.getItem("accessToken");
      
      const response = await fetch(
        `${BASE_API}/Doctor/assign-visit`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            timeSlotId: timeSlotId,
            VisitRequestId: selectedRequest.id,
          }),
        }
      );
      
      if (!response.ok) throw new Error("Failed to assign time");
      
      handleCloseCalendar();
    } catch (error) {
      console.error("Error assigning time:", error);
    } finally {
      setLoadingAssign(false);
    }
  };
  
  return (
    <Box>
      <Typography variant="h6" sx={{ mb: 2 }}>
        Пацієнти, що очікують підтвердження
      </Typography>
      
      {loading && (
        <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
          <CircularProgress />
        </Box>
      )}
      
      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}
      
      {!loading && !error && requests.length > 0 ? requests.map((request) => (
        <VisitRequestCard 
          key={request.id}
          request={request}
          onAssignTime={handleOpenCalendar}
        />
      )) : (
        !loading && !error && 
        <Typography sx={{ textAlign: 'center', py: 4, color: 'text.secondary' }}>
          Немає запитів, що очікують підтвердження
        </Typography>
      )}
      
      <Drawer
        anchor="right"
        open={showCalendarModal}
        onClose={handleCloseCalendar}
        sx={{
          '& .MuiDrawer-paper': { 
            width: { xs: '100%', sm: 400 },
            boxSizing: 'border-box',
          },
        }}
      >
        {loadingAssign ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
            <CircularProgress />
          </Box>
        ) : (
          <Calendar 
            onTimeSelect={handleTimeSelect}
            onClose={handleCloseCalendar}
          />
        )}
      </Drawer>
    </Box>
  );
}