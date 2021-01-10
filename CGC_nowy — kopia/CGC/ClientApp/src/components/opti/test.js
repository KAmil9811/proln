import React, { Component } from "react";
import './test.css';
import { OptiTable } from './OptiTable'
import { OptiTableItems } from './OptiTableItems'


export class Test extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2:'',
            table: [],

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
                for(var i = 0; i < json.length; i++) {
                    /*table2.push({
                        length: json[i].length,
                        width: json[i].width,
                        thickness: json[i].hight,
                        color: json[i].color,
                        type: json[i].type,
                        ids: json[i].id,
                        status: json[i].status,
                        desk: json[i].desk,
                    })*/
                    var canvas = document.getElementById('canvas');
                    if (canvas.getContext) {
                        var ctx = canvas.getContext('2d');
                        if (json[i].width > 999 || json[i].length > 999) {
                            ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                            ctx.fillRect(0, 1700 * i, (json[i].width), (json[i].length)); ///tafla
                          
                        }
                        else {
                            ctx.strokeRect(0, 1700 * i, json[i].width, json[i].length); ///tafla
                            //ctx.fillStyle = "rgba(9, 157, 215, 0.7)";
                        }
                        
                        if (json[i].width === 0) {
                            sessionStorage.setItem('uncat', json[i].error_Messege)

                        }
                        else {
                            for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                ctx.strokeRect(json[i].glass_info[0].pieces[j].x, json[i].glass_info[0].pieces[j].y+(1700*i), json[i].glass_info[0].pieces[j].widht, json[i].glass_info[0].pieces[j].lenght);///itemy
                            }
                        }
                    }
                    console.log('beka z MimeTypeArray')
                };

                
                //return (json)
            })






        /*var canvas = document.getElementById('canvas');
        var x = 0;
        var y = 0;
        var w = 50;
        var h = 50;
        /////
        var x2 = 50;
        var y2 = 0;
        var w2 = 200;
        var h2 = 250;
        /////
        var x3 = 0;
        var y3 = 50;
        var w3 = 50;
        var h3 = 50;
        
        if (canvas.getContext) {
            var ctx = canvas.getContext('2d');
            //ctx.fillText("tekst", x, y);
            ctx.font = "40px Arial";
            ctx.fillText("450x450", 0, 40);
            ctx.font = "10px Arial";
            ctx.strokeRect(0, 50, 450, 450); ///tafla
            ctx.strokeRect(x, y+50, w, h);///itemy
            ctx.fillText("50x50", x+5, 60);
            ctx.strokeRect(x2, y2+50, w2, h2);
            ctx.fillText("200x250", x2 + 5, y2+60);
            ctx.strokeRect(x3, y3+50, w3, h3);
            ctx.fillText("50x50", x3 + 5, y3+60);
        }*/
    }

   


    

    render() {
        return (
            <div>
                <div className="canva">
<<<<<<< HEAD
                    <canvas className="canvas" id="canvas" width="3000" height="3000" ></canvas>
=======
                    <canvas className="canvas" id="canvas" width="10000" height="10000" ></canvas>
>>>>>>> 3cd2ea586d30812a7b0831fbe25bf57f5bf8c640
                </div>
                <h2>{sessionStorage.getItem('uncat')}</h2>
                <div className="table2">
                    <h2>Tafle</h2>
                    <OptiTable />
                </div>
                <div className="table3">
                    <h2>Produkty</h2>
                    <OptiTableItems />
                    <button className="add_machine" >Zapisz projekt</button>
                    <button className="add_machine" >Wytnij</button>
                </div>
            </div>
        );
    }
}

