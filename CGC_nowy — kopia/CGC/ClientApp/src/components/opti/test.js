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
                var table2 = [];
                table2 = json
                for (var i = 0; i < table2.length; i++) {
                    //table2.push({id:json[i].glass_info[0].id})
                    var canvas = document.getElementById('canvas');
                    if (canvas.getContext) {
                        var ctx = canvas.getContext('2d');
                        if ((table2[i].width >= 100000 && table2[i].width <= 999999) || (table2[i].length >= 100000 && table2[i].length <= 999999)) {
                            if (i == 0) {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, 100, (table2[i].width / 100), (table2[i].length / 100));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 100, (json[i].glass_info[0].pieces[j].y / 100) + 100, json[i].glass_info[0].pieces[j].widht / 100, json[i].glass_info[0].pieces[j].lenght / 100);///itemy
                                    }
                                }
                                this.setState({
                                    position: 100 + (table2[i].length / 100)
                                })

                            }
                            else {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, this.state.position + 200, (table2[i].width / 100), (table2[i].length / 100));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 100, (((json[i].glass_info[0].pieces[j].y / 100) + this.state.position)) + 200, json[i].glass_info[0].pieces[j].widht / 100, json[i].glass_info[0].pieces[j].lenght / 100);///itemy
                                    }
                                }
                                this.setState({
                                    position: this.state.position + 200 + (table2[i].length / 100)
                                })

                            }

                        }
                        else if ((table2[i].width >= 10000 && table2[i].width <= 99999) || (table2[i].length >= 10000 && table2[i].length <= 99999)) {
                            if (i == 0) {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, 30, (table2[i].width / 100), (table2[i].length / 100));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 100, (json[i].glass_info[0].pieces[j].y / 100) + 30, json[i].glass_info[0].pieces[j].widht / 100, json[i].glass_info[0].pieces[j].lenght / 100);///itemy
                                    }
                                }
                                this.setState({
                                    position: 30 + (table2[i].length / 100)
                                })

                            }
                            else {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, this.state.position + 30, (table2[i].width / 100), (table2[i].length / 100));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 100, (((json[i].glass_info[0].pieces[j].y / 100) + this.state.position)) + 30, json[i].glass_info[0].pieces[j].widht / 100, json[i].glass_info[0].pieces[j].lenght / 100);///itemy
                                    }
                                }
                                this.setState({
                                    position: this.state.position + 30 + (table2[i].length / 100)
                                })
                            }
                        }
                        else if ((table2[i].width >= 999 && table2[i].width <= 9999) || (table2[i].length >= 999 && table2[i].length <= 9999)) {
                            if (i == 0) {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, 100, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 10, (json[i].glass_info[0].pieces[j].y / 10) + 100, json[i].glass_info[0].pieces[j].widht / 10, json[i].glass_info[0].pieces[j].lenght / 10);///itemy
                                    }
                                }
                                this.setState({
                                    position: 100 + (table2[i].length / 10)
                                })

                            }
                            else {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, this.state.position + 200, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    console.log('table2[i].width >= 999 && table2[i].width <= 9999) || (table2[i].length >= 999 && table2[i].length <= 9999)')
                                    console.log(this.state.position)
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 10, (((json[i].glass_info[0].pieces[j].y/10) + this.state.position)) + 200, json[i].glass_info[0].pieces[j].widht / 10, json[i].glass_info[0].pieces[j].lenght / 10);///itemy


                                       /* var yse = (json[i].glass_info[0].pieces[j].y + this.state.position) + 200;
                                        var wid = json[i].glass_info[0].pieces[j].widht / 10;
                                        var len = json[i].glass_info[0].pieces[j].lenght / 10; 
                                        console.log('x=' + json[i].glass_info[0].pieces[j].x / 10 + ' ' + 'y= ' + yse + ' ' + 'width= ' + wid + ' ' + 'lenght=' + len)*/
                                    }
                                }
                                this.setState({
                                    position: this.state.position + 200 + (table2[i].length / 10)
                                })
                            }
                        }
                        else {
                            if (i == 0) {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, 100, (table2[i].width), (table2[i].length));
                                
                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x, json[i].glass_info[0].pieces[j].y + 100, json[i].glass_info[0].pieces[j].widht, json[i].glass_info[0].pieces[j].lenght);///itemy
                                    }
                                }
                                this.setState({
                                    position: 100 + (table2[i].length )
                                })

                            }
                            else {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, this.state.position + 100, (table2[i].width ), (table2[i].length ));
                                
                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x, (((json[i].glass_info[0].pieces[j].y) + this.state.position)) + 100 , json[i].glass_info[0].pieces[j].widht, json[i].glass_info[0].pieces[j].lenght);///itemy
                                    }
                                }
                                this.setState({
                                    position: this.state.position + 100 + (table2[i].length)
                                })


                            }
                        }
                        ///////////////////// Koniec rysowania tafli
                        ///////////////////// Rysowanie itemów
                        /*if (json[i].width === 0) {
                            sessionStorage.setItem('uncat', json[i].error_Messege)

                        }
                        else {
                            for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                ctx.strokeRect(json[i].glass_info[0].pieces[j].x, json[i].glass_info[0].pieces[j].y + (2000 * i), json[i].glass_info[0].pieces[j].widht, json[i].glass_info[0].pieces[j].lenght);///itemy
                            }
                        }*/
                    }
                };

                this.setState({
                    glass_ids: table2
                })
                //return (json)
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

/*<script src="//cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.3/jspdf.min.js"></script>   onClick={this.function}  x={1} y={1} scale={0.3}*/

    render() {
        const ref = React.createRef();
        const options = {
            orientation: 'landscape',
           /* unit: 'in',
            format: [4, 2]*/
        };
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Zaloguj się, aby usyskać dostęp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (
                <div>
                    <Sidebar />
                    <div className="test_c">
                        <div className="canva">
                            <canvas className="canvas" ref={ref} id="canvas" width="10000" height="10000" ></canvas>

                        </div>
                        <h3>{sessionStorage.getItem('uncat')}</h3>
                        <div className="table2">
                            <h2>Tafle</h2>
                            <OptiTable />
                        </div>
                        <div className="table3">
                            <h2>Produkty</h2>
                            <OptiTableItems />
                            <div>
                                <button className="prim_test" onClick={this.saveProject}>Zapisz projekt</button>
                                <button className="success_test" onClick={this.cutOrder}>Zapisz i wytnij</button>
                                <ReactToPdf targetRef={ref} filename="div-blue.pdf" options={options} >
                                    {({ toPdf }) => (
                                        <button className="success_test" onClick={toPdf} >Wygeneruj PDF projektu</button>
                                    )}
                                </ReactToPdf>
                            </div>
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

