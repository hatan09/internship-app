import React, { useState, useEffect } from 'react';
import JobItem from '../../JobItem';
import {getAllJobByDep, getSkill, getComp, apply} from '../../../api/Api'
import Modal from 'react-modal';
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
const listHeaders = {id:0, title: 'Job Title', company: 'Company Name', slots: 'Available Slots'};

function StudentJobPage() {
    const [skillData, setSkillData] = useState([]);
    const [jobListData, setJobListData] = useState([]);
    const [jobData, setjobData] = useState([]);
    const [modalIsOpen, setIsOpen] = useState(false);

    async function loadData () {
        const depId = localStorage.getItem('depId');
        var jobs = await getAllJobByDep(depId);
        jobs.map(async (job, index) => {
            var comp = await getComp(job.companyId);
            jobs[index]['company'] = comp.title;
            setJobListData(jobs);
        })
    }

    useEffect(() => {
        loadData();
    }, []);

    async function openModal(job) {
        setjobData(job);
        var ids = job.skillIds;
        const skills = [];
        ids.map(async (id) => {
            var skill = await getSkill(id);
            skills.push(skill);
            setSkillData(skills);
        });
        setIsOpen(true);
      }

      async function handleApply(){
        const stuId = localStorage.getItem("userId");
        const jobId = jobData.id;
        console.log(stuId , jobId);
        var result = await apply(stuId, jobId);
        closeModal();
      }
    
      function afterOpenModal() {
      }
    
      function closeModal() {
        setIsOpen(false);
      }

    return (
        <div className="student-job-page">
            <div className="job-list">
                <div className="list-title">
                    <span>Available Jobs</span>
                </div>
                <div className="search">
                    <form action="search">
                        <input type="text" name="search-box" id="serach-box" placeholder='Search Job' />
                    </form>
                </div>
                <br />
                <ul className="list">
                    <li id={listHeaders.id} style={{fontWeight: 'bold'}}> <JobItem item={listHeaders}></JobItem></li>
                    <hr />
                    {jobListData.map(item => (
                        <li key={item.id}>
                            <JobItem item={item} onClick={openModal}/>
                        </li>
                    ))}
                </ul>
            </div>

            <Modal
                isOpen={modalIsOpen}
                onAfterOpen={afterOpenModal}
                onRequestClose={closeModal}
                style={customStyles}
                contentLabel="Example Modal"
            >
                <h1>Job Information</h1>
                <h3>{jobData.title}</h3>
                <p>At: <span style={{fontWeight: 'bold'}}>{jobData.company}</span></p>
                <p>Minimum Credits: {jobData.minCredit}</p>
                <p>Minimum GPA: {jobData.minGPA}</p>
                <p>Available slots: {jobData.slots}</p>
                <p>Required Skills:</p>
                <ul>
                    {skillData.map(skill => (
                        <li style={{marginLeft: "20px"}} key={skill.id}> - {skill.name}</li>
                    ))}
                </ul>
                
                <button style={{padding: "3px", fontSize: "12pt", backgroundColor: "green"}} onClick={handleApply}>Apply</button>
                <button style={{float: "right", padding: "3px", fontSize: "12pt", backgroundColor: "red"}} onClick={closeModal}>Close</button>
                
            </Modal>
        </div>
     );
}

export default StudentJobPage;
