import { useState, useEffect } from 'react';
import VisitRequestCard from '../VisitRequestCard';
import { fetchVisitRequests } from '../../../services/Admin/adminService';
import { assignDoctorToVisit } from '../../../services/Admin/assignDoctorToVisit';
import { Pagination, Box, Typography, Button, CircularProgress, Alert } from '@mui/material';

export default function RequestsTab() {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  
  // Добавление состояний для пагинации
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const requestsPerPage = 6; // Количество запросов на странице

  const handleFetchRequests = async () => {
    setLoading(true);
    setError(null);

    try {
      const data = await fetchVisitRequests();
      console.log('Запити на візити:', data);
      setRequests(data);
      
      // Расчет общего количества страниц для пагинации
      setTotalPages(Math.ceil(data.length / requestsPerPage));
    } catch (err) {
      setError('Помилка при отриманні даних');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleAssignDoctor = async (doctorId, visitId) => {
    setLoading(true);
    setError(null);

    try {
      await assignDoctorToVisit(doctorId, visitId);
      await handleFetchRequests();
      alert('Лікаря успішно призначено');
    } catch (err) {
      setError('Помилка при призначенні лікаря');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // Обработчик изменения страницы
  const handlePageChange = (event, value) => {
    setPage(value);
  };

  useEffect(() => {
    handleFetchRequests();
  }, []);

  // Получение текущих запросов для отображения на странице
  const getCurrentRequests = () => {
    const startIndex = (page - 1) * requestsPerPage;
    const endIndex = startIndex + requestsPerPage;
    return requests.slice(startIndex, endIndex);
  };

  return (
    <Box sx={{ padding: 2 }}>
      <Box sx={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center', 
        marginBottom: 3 
      }}>
        <Typography variant="h5" component="h1">Запити на візити</Typography>
        <Button 
          variant="contained" 
          onClick={handleFetchRequests} 
          disabled={loading}
        >
          {loading ? 'Завантаження...' : 'Оновити запити'}
        </Button>
      </Box>

      {error && <Alert severity="error" sx={{ marginBottom: 2 }}>{error}</Alert>}

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', padding: 4 }}>
          <CircularProgress />
        </Box>
      ) : requests.length === 0 ? (
        <Box sx={{ 
          display: 'flex', 
          flexDirection: 'column', 
          alignItems: 'center', 
          padding: 4 
        }}>
          <img
            src="/no-requests.svg"
            alt="Немає запитів"
            style={{ maxWidth: '200px', marginBottom: 2 }}
          />
          <Typography>Наразі немає запитів на допомогу</Typography>
        </Box>
      ) : (
        <>
          <Box sx={{ 
            display: 'grid', 
            gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr', lg: '1fr 1fr 1fr' },
            gap: 3,
            marginBottom: 3
          }}>
            {getCurrentRequests().map((request) => (
              <VisitRequestCard
                key={request.id}
                request={request}
                onAssignDoctor={handleAssignDoctor}
              />
            ))}
          </Box>
          
          {/* Пагинация */}
          <Box sx={{ display: 'flex', justifyContent: 'center', marginTop: 3 }}>
            <Pagination 
              count={totalPages} 
              page={page} 
              onChange={handlePageChange} 
              color="primary" 
              showFirstButton 
              showLastButton
            />
          </Box>
        </>
      )}
    </Box>
  );
}