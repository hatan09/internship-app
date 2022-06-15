const baseUrl = "https://localhost:44312/api";
// const baseUrl = "https://";

export const getComp = async (id) => {
    const response = await fetch(`${baseUrl}/company/get/${id}`);
    const data = await response.json();
    return data;
};
// instructor
export const getIns = async (id) => {
    const response = await fetch(`${baseUrl}/instructor/get/${id}`);
    const data = await response.json();
    return data;
};

export const getGroup = async (insId) => {
    const response = await fetch(`${baseUrl}/interngroup/getbyinstructor/${insId}`);
    const data = await response.json();
    return data;
};

export const getAllReg = async (grpId) => {
    const response = await fetch(`${baseUrl}/student/getregister/${grpId}`);
    const data = await response.json();
    return data;
};

export const updateRegStat = async (regId, stat) => {   // 0 = pending, 1 = accepted, 2 = denied
    const response = await fetch(`${baseUrl}/student/updateStatus`, {
        headers: { "Content-Type": "application/json" },
        method: "PUT",
        body: JSON.stringify({
            id: regId,
            stat: stat,
        }),
    });
    const data = response.status;
    return data;
};
  
export const getAllStudent = async (depId) => {
    const response = await fetch(`${baseUrl}/student/GetAllByGroup/${depId}`);
    const data = await response.json();
    return data;
};

export const getAllJobByDep = async (depId) => {
    const response = await fetch(`${baseUrl}/job/getallbydepartment/${depId}`);
    const data = await response.json();
    return data;
};
// end instructor

// student
export const getStudent = async (id) => {
    const token = localStorage.getItem('token');
    //console.log(token);
    const response = await fetch(`${baseUrl}/student/get/${id}`, {
        headers: {
            'Authorization': `Bearer ${token}`,
            'Access-Control-Allow-Origin': '*'
        },
        method: "GET"
    });
    const data = await response.json();
    return data;
};

export const getSkill = async (id) => {
    const response = await fetch(`${baseUrl}/skill/get/${id}`);
    const data = await response.json();
    return data;
};

export const serachJob = async (name) => {
    const response = await fetch(`${baseUrl}/job/search/${name}`);
    const data = await response.json();
    return data;
};

export const apply = async (stuId, jobId) => {
    const response = await fetch(`${baseUrl}/student/apply`, {
        headers: { "Content-Type": "application/json" },
        method: "PUT",
        body: JSON.stringify({
            studentId: stuId,
            jobId: jobId,
        }),
    });
    const data = response.status;
    return data;
};

export const getAllGroup = async () => {
    const response = await fetch(`${baseUrl}/interngroup/getall`);
    const data = await response.json();
    return data;
};
// end student

// recruiter
export const getRec = async (id) => {
    const response = await fetch(`${baseUrl}/recruiter/get/${id}`);
    const data = await response.json();
    return data;
};

export const getAllJobByCom = async (comId) => {
    const response = await fetch(`${baseUrl}/job/GetAllByCompany/${comId}`);
    const data = await response.json();
    return data;
};

export const createJob = async (job) => {
    const response = await fetch(`${baseUrl}/job/create`, {
        headers: { "Content-Type": "application/json" },
        method: "POST",
        body: JSON.stringify(job),
    });
    const data = await response.json();
    return data;
};

export const getAllApplicantByJob = async (jobId) => {
    const response = await fetch(`${baseUrl}/student/getAllByJob/${jobId}`);
    const data = await response.json();
    return data;
};

export const accept = async (stuId, jobId) => {
    const response = await fetch(`${baseUrl}/student/accept`, {
        headers: { "Content-Type": "application/json" },
        method: "PUT",
        body: JSON.stringify({
            studentId: stuId,
            jobId : jobId
        }),
    });
    const data = response.status;
    return data;
};

export const reject = async (stuId, jobId) => {
    const response = await fetch(`${baseUrl}/student/reject`, {
        headers: { "Content-Type": "application/json" },
        method: "PUT",
        body: JSON.stringify({
            studentId: stuId,
            jobId : jobId
        }),
    });
    const data = await response.json();
    return data;
};


export const login = async (username, pass) => {
    const response = await fetch(`${baseUrl}/auth/login`, {
        headers: { "Content-Type": "application/json" },
        method: "POST",
        body: JSON.stringify({username: username, password: pass}),
    });
    const data = await response.json();
    return data;
};

export const reg = async (user) => {
    console.log(user);
    const response = await fetch(`${baseUrl}/student/create`, {
        headers: { "Content-Type": "application/json" },
        method: "POST",
        body: JSON.stringify(user),
    });
    const data = await response.json();
    return data;
};
