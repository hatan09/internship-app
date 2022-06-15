import React, { useState, useEffect } from 'react';
import './AddJobPage.css';
import JobItem from '../../JobItem';
import Modal from 'react-modal';
import { getAllJobByCom, getComp, createJob } from '../../../api/Api';
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


function AddJobPage() {
    const [listJob, setListJob] = useState([]);
    const [company, setCompany] = useState('');
    const [modalIsOpen, setIsOpen] = useState(false);

    async function loadData () {
        var comId = localStorage.getItem('comId');
        var comp = await getComp(comId);
        setCompany(comp.title);
        var jobs = await getAllJobByCom(comId);
        jobs.map(async (job, index) => {
            jobs[index]['company'] = comp.title;
            
        })
        setListJob(jobs);
    }

    useEffect(() => {
        loadData();
    }, []);

    async function onAddJob(event){
        // Prevent page reload
        event.preventDefault();

        const comId = localStorage.getItem('comId');
        var { title, minCredit, minGpa, slots } = document.forms[0];
        var result = await createJob({
            title: title.value,
            minGPA: minGpa.value,
            slots: slots.value,
            minCredit: minCredit.value,
            companyId: comId,
            departmentIds: [1],
            skillIds: []
        });
        closeModal();
    }

    function openModal() {
        setIsOpen(true);
      }
    
      function afterOpenModal() {
      }
    
      function closeModal() {
        setIsOpen(false);
      }

    return ( 
        <div className="add-job-page">
            <div className="job-list">
                <div className="list-title">
                    <span>Existing Jobs</span>
                </div>
                <ul className="list">
                    <li id={listHeaders.id} style={{fontWeight: 'bold'}}> <JobItem item={listHeaders}></JobItem></li>
                    <hr />
                    {listJob.map(item => (
                        <li key={item.id}>
                            <JobItem item={item}/>
                        </li>
                    ))}
                </ul>
                <br />
                <br />
                <div className="add-job">
                    <span>Need To Add A Job?</span>
                    <button className='add-job-button' onClick={openModal}> + Add More Jobs </button>
                </div>
            </div>

            <Modal
                isOpen={modalIsOpen}
                onAfterOpen={afterOpenModal}
                onRequestClose={closeModal}
                style={customStyles}
                contentLabel="Example Modal"
            >
                <h1>Job Information</h1>
                <form onSubmit={onAddJob}>
                    <h3>{company}</h3>
                    <span>Job's Name:</span> <input type="text" name="title" id="name" />
                    <span>Minumum Credits:</span> <input type="number" name="minCredit" id="credit" />
                    <span>Minumum GPA:</span> <input type="number" name="minGpa" id="gpa" />
                    <span>Available slots:</span> <input type="number" name="slots" id="slots" />
                    <button style={{padding: "3px", fontSize: "12pt", backgroundColor: "green"}} type='Submit'>+ Add</button>
                </form>
                
                
                
                <button style={{float: "right", padding: "3px", fontSize: "12pt", backgroundColor: "red"}} onClick={closeModal}>Close</button>
                
            </Modal>
        </div>
     );
}

export default AddJobPage;
