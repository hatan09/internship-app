import React, { useState, useEffect } from 'react';
import {getIns, getAllJobByDep, getComp, getSkill} from '../../../api/Api'
import JobItem from '../../JobItem';
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
const listHeaders = {id:0, title: 'Job Name', company: 'Company Name', slots: 'Available Slots'};
const list = [
    {
        id: '1', 
        name: 'ReactJS Developer', 
        company: 'IdeaLogic Inc.',
        credit: 56, 
        gpa: 70, 
        slots: 100, 
        skills: [{id:1, name: 'C#', level:'Advance'}, {id:2, name: 'ReactJS', level:'Beginner'}],
        department: [{}]}
];

function ViewJobPage() {
    const [skillData, setSkillData] = useState([]);
    const [jobListData, setJobListData] = useState([]);
    const [jobData, setjobData] = useState(list[0]);
    const [modalIsOpen, setIsOpen] = useState(false);

    async function loadData(){
        var id = localStorage.getItem('userId');
        var ins = await getIns(id);
        var depId = ins.departmentId;
        var jobs = await getAllJobByDep(depId);
        jobs.map(async (job, index) => {
            var comp = await getComp(job.companyId);
            jobs[index]['company'] = comp.title;
        })
        setJobListData(jobs);
    }

    useEffect(() => {
        loadData();
    }, []);

    function openModal(job) {
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
    
      function afterOpenModal() {
      }
    
      function closeModal() {
        setIsOpen(false);
      }

    return (
        <div className="view-job-page">
            <div className="job-list">
                <div className="list-title">
                    <span>View Jobs</span>
                </div>
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
                <p>{jobData.company}</p>
                <p>Minimum Credits: {jobData.minCredit}</p>
                <p>Minimum GPA: {jobData.minGPA}</p>
                <p>Available slots: {jobData.slots}</p>
                <p>Required Skills:</p>
                <ul>
                    {skillData.map(skill => (
                        <li style={{marginLeft: "20px"}} key={skill.id}> - {skill.name}</li>
                    ))}
                </ul>
                
                <button style={{float: "right", padding: "3px", fontSize: "12pt", backgroundColor: "red"}} onClick={closeModal}>Close</button>
                
            </Modal>
        </div>
     );
}

export default ViewJobPage;
