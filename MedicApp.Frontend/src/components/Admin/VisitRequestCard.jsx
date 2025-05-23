import NearestDoctorsModal from './NearestDoctorsModal';
import { useState } from 'react';
import { fetchNearestDoctors } from '../../services/Admin/fetchNearestDoctors';

export default function VisitRequestCard({ request, onAssignDoctor }) {

    const [showDoctorsModal, setShowDoctorsModal] = useState(false);
    const [nearestDoctors, setNearestDoctors] = useState([]);
    const [loading, setLoading] = useState(false);

    const handleAssignDoctorClick = async () => {
        setLoading(true);
        try {
            const doctors = await fetchNearestDoctors(request.id);

            console.log(doctors);

            if (doctors) {
                setNearestDoctors(doctors);
                setShowDoctorsModal(true);
            } 
        } catch (error) {
            console.error('Помилка при отриманні списку лікарів:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleSelectDoctor = (doctorId) => {
        console.log('ID лікаря:', doctorId, 'ID запиту:', request.id);
        onAssignDoctor(doctorId, request.id); 
        setShowDoctorsModal(false);
    };

   return (
       <div className="request-card">
           <div className="card-header">
               <div className="header-content">
                   <h3>Візит #{request.id}</h3>
                   <div className="menu-dots">⋮</div>
               </div>
               <div className="patient-id">
                   Пацієнт ID: {request.patienId}
               </div>
           </div>

           <div className="card-content">
               <div className="info-row">
                   <span className="label">Опис:</span>
                   <span>{request.description}</span>
               </div>

               <div className="info-row">
                   <span className="label">Адреса:</span>
                   <span>
                       {request.address ? 
                           `${request.address.city}, ${request.address.street}, буд. ${request.address.building}, кв. ${request.address.appartaments}, ${request.address.country}` 
                           : 'Не вказано'}
                   </span>
               </div>
           </div>

          <div className="card-footer">
                <span className="status-badge">Очікує</span>
                <button 
                    className="assign-doctor-button"
                    onClick={handleAssignDoctorClick}
                    disabled={loading}
                >
                    {loading ? 'Завантаження...' : 'Призначити лікаря'}
                </button>
            </div>

            <NearestDoctorsModal
                isOpen={showDoctorsModal}
                onClose={() => setShowDoctorsModal(false)}
                doctors={nearestDoctors}
                onSelectDoctor={handleSelectDoctor}
            />
       </div>
   );
}
