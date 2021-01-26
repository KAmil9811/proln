import React, { Component } from "react";
import './test.css';
import { OptiTable } from './OptiTable'
import { OptiTableItems } from './OptiTableItems'
import Sidebar from '../Sidebar';
import jsPDF from 'jspdf';
import ReactToPdf from "react-to-pdf";

export class Test extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2: '',
            table: [],
            pieces: [],
            glass_ids: [],
            position: '',
        }
    }

    componentDidMount() {

        const receiver = {

            order: {
                id_order: sessionStorage.getItem('idOpti'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            },
            item: {
                color: sessionStorage.getItem('colorOpti'),
                type: sessionStorage.getItem('typeOpti'),
                thickness: sessionStorage.getItem('thicknessOpti'),

            }
        }
        fetch(`api/Cut/Magic`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                
            })
    }

    
    cutOrder = (event) => {
        event.preventDefault();
        const receiver = {
            glasses:
                this.state.glass_ids
            ,
            order: {
                id_order: sessionStorage.getItem('idOpti')
            }

        }


        fetch(`api/Cut/Save_Project`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(receiver)
                console.log(json)
                return (json)
            })
            .then(json => {
                sessionStorage.setItem('id_order', json);
            })
        this.props.history.push('/pick_machine');

        console.log(receiver)
    }

    //api/Cut/Save_Project
    saveProject = (event) => {
        event.preventDefault();
        const receiver = {
            glasses:
                this.state.glass_ids
            ,
            order: {
                id_order: sessionStorage.getItem('idOpti')
            }

        }


        fetch(`api/Cut/Save_Project`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(receiver)
                console.log(json)
                sessionStorage.setItem('id_order', json.order.id_order);
                return (json)
            })
        //this.props.history.push('/glasswarehouse');

        console.log(receiver)

    }

    generator = (event) => {
        fetch(`api/Cut/CreatePdf`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json);
            })
        document.getElementById
}
    open = (event) => {
        event.preventDefault();
    }

    render() {
        const ref = React.createRef();
       
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('cutManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div >
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Cut project</h1>
                    </div>
                    
                        <h3>{sessionStorage.getItem('uncat')}</h3>
                        <div className="table2">
                            <h2>Glasses</h2>
                            <OptiTable />
                        </div>
                        <div className="table3">
                            <h2>Products</h2>
                            <OptiTableItems />
                            <div>
                                <button className="prim_test" onClick={this.saveProject}>Save project</button>
                            <button className="success_test" onClick={this.cutOrder}>Save and cut</button>
                            <button className="success_test" onClick={this.generator} >Generate PDF </button>
                            <button className="success_test" onClick={this.open} >hhh </button>
                            <a href="/download" title="Download" download="./Project.pdf">hjbbhhvhv</a>
                            </div>
                        </div>
                    </div>
            );
        }
        else {
            
                return (
                <div >
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Cut project</h1>
                    </div>
                    <div className="test_c">
                        <div className="canva">
                            <canvas className="canvas" ref={ref} id="canvas" width="10000" height="10000" ></canvas>

                        </div>
                        <h3>{sessionStorage.getItem('uncat')}</h3>
                        <div className="table2">
                            <h2>Glasses</h2>
                            <OptiTable />
                        </div>
                        <div className="table3">
                            <h2>Products</h2>
                            <OptiTableItems />
                        </div>

                    </div>
                </div>
            );
        }
    }

    function() {
        // only jpeg is supported by jsPDF
        var canvas = document.getElementById("canvas");
        var imgData = canvas.toDataURL("image/jpeg", 0.5);
        var pdf = new jsPDF();

        pdf.addImage(imgData, 'JPEG', 0, 0);// te zmienne odpowiadają za przesuniecie względem lewego górnego rogu 
        pdf.save("download.pdf");
    }
}

