import { useState, useEffect } from 'react';
import '../../styles/Doctor/DoctorDashboard.css';
import { fetchVisitPendingRequests } from '../../services/Doctor/fetchPendingVisitRequests';
import ChatTab from '../../components/Shared/Chat/ChatTab';
import Sidebar from '../../components/Shared/Sidebar/Sidebar';
import RequestForConfirm from '../../components/Doctor/featureTab/TabForConfirmRequest';
import ViewListIcon from '@mui/icons-material/ViewList';
import ChatIcon from '@mui/icons-material/Chat';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import PersonIcon from '@mui/icons-material/Person';
import TabSchedule from '../../components/Doctor/featureTab/TabSchedule';
import TabPatientDetail from '../../components/Doctor/featureTab/TabPatientDetail';
export default function DoctorDashboard() {
    const [activeTab, setActiveTab] = useState('pending');
    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        if (activeTab === 'pending') {
            handleFetchPendingRequests();
        }
    }, [activeTab]);

    const handleFetchPendingRequests = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await fetchVisitPendingRequests();

            console.log(data); 
            setRequests(data);
        } catch (err) {
            setError('Помилка при отриманні даних');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const tabsConfig = [
        {
            key: 'pending',
            label: 'Очікують',
            icon: <ViewListIcon style={{ fontSize: 40, color: '#4caf50' }} />,
            component:  <RequestForConfirm requests={requests} loading={loading} error={error} />
        },
        {
            key: 'schedule',
            label: 'Розклад',
            icon: <CalendarTodayIcon style={{ fontSize: 40, color: '#4caf50' }}/>,
            component: <TabSchedule/>
        },
        {
            key: 'patients',
            label: 'Пацієнти',
            icon: <PersonIcon style={{ fontSize: 40, color: '#4caf50' }}/>,
            component: <TabPatientDetail/>
        }
    ];

    return (
        <div className="dashboard-container">
            <Sidebar
                tabsConfig={tabsConfig}
                activeTab={activeTab}
                setActiveTab={setActiveTab}
            />

            <div className="main-content">
                {tabsConfig.map(tab => (
                    tab.key === activeTab && (
                        <div key={tab.key}>
                            {tab.component}
                        </div>
                    )
                ))}
            </div>
        </div>
    );
}
