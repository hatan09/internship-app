import {React, useState, useEffect} from 'react';
import {getAllApplicantByJob, getAllJobByCom, accept, reject} from '../../../api/Api'
import './StudentInfoPage.css';
import Modal from 'react-modal';
import ApplicantItem from '../../ApplicantItem';
Modal.setAppElement('#root');

const customStyles = {
    content: {
      top: '50%',
      left: '50%',
      right: 'auto',
      bottom: 'auto',
      marginRight: '-50%',
      transform: 'translate(-50%, -50%)',
    },
  };
  const listHeaders = {id:0, fullName: 'Name', birthdate: 'Year', status: 'Status'};

function StudentInfoPage() {
    const [jobId, setJobId] = useState();
    const [jobListData, setJobListData] = useState([]);
    const [modalIsOpen, setIsOpen] = useState(false);
    const [applicant, setApplicant] = useState([]);

    async function loadData () {
        var comId = localStorage.getItem('comId');
        var jobs = await getAllJobByCom(comId);
        var tempJobList = [];
        jobs.map(async job => {
            var applicants = await getAllApplicantByJob(job.id);
            job['applicants'] = applicants;
            tempJobList.push(job);
        });
        setJobListData(tempJobList);
    }

    useEffect(() => {
        loadData();
    }, []);

    async function handleAccept(){
        const stuId = applicant.id;
        var result = await accept(stuId, jobId);
    }

    async function handleReject(){
        const stuId = applicant.id;
        var result = await reject(stuId, jobId);
    }

    function openModal(applicant, jobId) {
        setApplicant(applicant);
        setJobId(jobId);
        setIsOpen(true);
      }
    
      function afterOpenModal() {
      }
    
      function closeModal() {
        setIsOpen(false);
      }

    return ( 
        <div className="student-info-page">
            <div className="student-list">
            <button onClick={openModal}>.</button>
                <div className="list-title">
                
                    <span>Applicant List</span>
                </div>
                {jobListData.map(job => (
                    <ul className="list">
                        <hr />
                        <li key={job.id}>{job.title}</li>
                        <li style={{fontWeight: 'bold'}}><ApplicantItem item={listHeaders}></ApplicantItem></li>
                        {job.applicants.map(item => (
                            <li key={item.id}>
                                <ApplicantItem item={item} jobId={job.id} onClick={openModal}/>
                            </li>
                        ))}
                    </ul>
                ))}
            </div>

            <Modal
                isOpen={modalIsOpen}
                onAfterOpen={afterOpenModal}
                onRequestClose={closeModal}
                style={customStyles}
                contentLabel="Example Modal"
            >
                <h1>Student Information</h1>
                <h3>{applicant.fullName}</h3>
                <p>{applicant.birthdate}</p>
                <p>{applicant.email}</p>
                <p>GPA: {applicant.gpa}</p>
                <p>Skills: </p>
                <ul>
                    {/* {studentData.skills.map(skill => (
                        <li style={{marginLeft: "20px"}} key={skill.id}> - {skill.name} ({skill.level})</li>
                    ))} */}
                </ul>
                

                <br />
                <button style={{padding: "3px", fontSize: "12pt", backgroundColor: "green"}} onClick={handleAccept}>Accpect</button>
                <button style={{marginLeft: "10px", padding: "3px", fontSize: "12pt", backgroundColor: "yellow"}} onClick={handleReject}>Reject</button>
                <br />
                <button style={{float: "right", padding: "3px", fontSize: "12pt", backgroundColor: "red"}} onClick={closeModal}>Close</button>
                
            </Modal>
        </div>
     );
}

export default StudentInfoPage;
