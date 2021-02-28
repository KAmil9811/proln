import React, { Component } from "react";
import './test.css';
import { OptiTable } from './OptiTable'
import { OptiTableItems } from './OptiTableItems'
import Sidebar from '../Sidebar';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';

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
            table: {
                columns: [],
                rows: []
            },
            table2: {
                columns: [],
                rows: []
            },
            jpegs:[],
        }
    }

    componentDidMount() {
        this.jpgPrint;
        var jpegvar = '';
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('idOpti'),
                color: sessionStorage.getItem('colorOpti'),
                type: sessionStorage.getItem('typeOpti'),
                thickness: sessionStorage.getItem('thicknessOpti'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            },
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
                return (json)
                
            })
            .then(json => {
               /* for (var i = 0; i < sessionStorage.getItem('ilosc'); i++) {
                    jpegvar = sessionStorage.getItem('login') + "_" + sessionStorage.getItem('orderId2') + "_" + sessionStorage.getItem('colorOpti') + "_" + sessionStorage.getItem('typeOpti') + "_" + sessionStorage.getItem('thicknessOpti') + "_" + i + ".jpg"
                    console.log(jpegvar)
                    sessionStorage.setItem('obrazek', jpegvar)
                    this.setState({
                        jpegs: this.state.jpegs.concat(jpegvar)
                    })
                }
                console.log('kur nie wiem')
                var aaa = this.state.jpegs
                console.log(aaa)*/



                var table3 = [];
                var table2 = [];
                sessionStorage.setItem('kolor', json[0].color)
                sessionStorage.setItem('typ', json[0].type)
                sessionStorage.setItem('grubosc', json[0].hight)
                sessionStorage.setItem('ilosc', json.length)
                for (var i = 0; i < json.length - 1; i++) {
                    table3.push({
                        length: json[i].length,
                        width: json[i].width,
                        thickness: json[i].hight,
                        color: json[i].color,
                        type: json[i].type,
                        ids: json[i].glass_info[0].id,
                        status: json[i].status,
                        desk: json[i].desk,
                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Length',
                                field: 'length',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Width',
                                field: 'width',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Thickness',
                                field: 'thickness',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Color',
                                field: 'color',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Type',
                                field: 'type',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'No.',
                                field: 'ids',
                                sort: 'asc',
                                width: 30
                            },
                        ],
                        rows: table3
                    }
                });
                sessionStorage.setItem('uncat2', json[json.length - 1].color)
                console.log('Uncat2====')
                console.log(sessionStorage.getItem('uncat2'))
                for (var i = 0; i < json.length-1; i++) {
                    for (var j = 0; j < json[i].glass_info.length; j++) {
                        for (var x = 0; x < json[i].glass_info[j].pieces.length; x++) {
                            table2.push({
                                length: json[i].glass_info[0].pieces[x].lenght,
                                width: json[i].glass_info[0].pieces[x].widht,
                                ids: json[i].glass_info[0].pieces[x].id,
                            })

                        }


                    }
                };
                this.setState({
                    table2: {
                        columns: [
                            {
                                label: 'Length',
                                field: 'length',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Width',
                                field: 'width',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'No.',
                                field: 'ids',
                                sort: 'asc',
                                width: 30
                            },
                        ],
                        rows: table2
                    }
                });
            })
       /* var table4 = [];
        for (var i = 0; i < sessionStorage.getItem('ilosc'); i++)  {
            table4.push(this.state.jpegs[i])
            
        }
        console.log("tablica czwarta")
        console.log(this.state.jpegs)
        var slides = table4
        var str = '<ul>'
        var aaaaaa = "Domi_1_green_normal_10_2.jpg"
        slides.forEach(function (slide) {
            str += '<li><img src="' + slide + '"/></li>';
        });
        str += '</ul>';

        document.getElementById("slideContainer").innerHTML = str;*/
            
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
              
                return (json)
            })
            .then(json => {
                sessionStorage.setItem('id_order', json);
            })
        this.props.history.push('/pick_machine');

       
    }

    saveProject = (event) => {
        event.preventDefault();
        const receiver = {
            glasses:
                this.state.glass_ids
            ,
            order: {
                id_order: sessionStorage.getItem('idOpti')
            },
            user: {
                login: sessionStorage.getItem('login'),
            },

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
                
                sessionStorage.setItem('id_order', json.order.id_order);
                return (json)
            })
        //this.props.history.push('/glasswarehouse');

       

    }

  generator = (event) => {
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('idOpti'),
                color:sessionStorage.getItem('colorOpti'),
                    type:sessionStorage.getItem('typeOpti'),
                thickness:sessionStorage.getItem('thicknessOpti'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            },
            glass_count: sessionStorage.getItem('ilosc')
        }
        fetch(`api/Cut/CreatePdf`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               
            })
        document.getElementById
    }




    table() {
        return (
            <MDBDataTableV5



                data={this.state.table}
                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={10}
                pagesAmount={10}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                sortable
                theadTextWhite
                theadTextWhite



            />
        )
    }

    table2() {
        return (
            <MDBDataTableV5



                data={this.state.table2}
                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={10}
                pagesAmount={10}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                sortable
                theadTextWhite
                theadTextWhite



            />
        )
    }
        
    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    render() {
        let table1 = this.table();
        let table2 = this.table2();
        var a = 'href'
        for (var i; i < sessionStorage.getItem('ilosc'); i++) {
            
            document.write(i)
            
        }
        let href = sessionStorage.getItem('uncat2')
        let href2 = "https://inzcgc.blob.core.windows.net/cgc/" + sessionStorage.getItem('login') + "_" + sessionStorage.getItem('orderId2') + "_" + sessionStorage.getItem('colorOpti') + "_" + sessionStorage.getItem('typeOpti') + "_" + sessionStorage.getItem('thicknessOpti') + ".pdf"
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
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
                    <div className="tescik" >
                        <h3>{sessionStorage.getItem('uncat')}</h3>
                            <div className="table2">
                                <h2>Glasses</h2>
                                {table1}
                             </div>
                        <div className="table3">
                            <h2>Products</h2>
                            {table2}
                            <img src={href} />
                            <div>
                                <button className="prim_test" onClick={this.saveProject}>Save project</button>
                                <button className="success_test" onClick={this.cutOrder}>Save and cut</button>
                                <a href={href2} download><button className="success_test" onClick={this.generator} >Generate PDF </button></a>
                            </div>
                            <div id="slideContainer"></div>
                        </div>
                    </div>
                </div>
            );
        }
        else {

            return (
                <div className="tescik" >
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Cut project</h1>
                    </div>
                    <div className="test_c">
                        <h3>{sessionStorage.getItem('uncat')}</h3>
                        <div className="table2">
                            <h2>Glasses</h2>
                            {table1}
                        </div>
                        <div className="table3">
                            <h2>Products</h2>
                            {table2}
                        </div>
                    </div>
                </div>
            );
        }
    }
}


